using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Library.Configuration;

namespace Library.Storage
{
    public class BinaryObjectManager : ITransientDependency
    {
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;
        private readonly AppFoldersConfiguration _appFolders;

        public BinaryObjectManager(IRepository<BinaryObject, Guid> binaryObjectRepository, AppFoldersConfiguration appFolders)
        {
            _binaryObjectRepository = binaryObjectRepository;
            _appFolders = appFolders;
        }

        public async Task<BinaryObject> GetOrNullAsync(Guid id)
        {
            return await _binaryObjectRepository.FirstOrDefaultAsync(id);
        }

        public async Task<BinaryObject> SaveAsync(Stream fileStream, string fileName, int? tenantId = null)
        {
            string filePath = Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString("D2"));
            string rootPath = Path.Combine(_appFolders.UploadFolder, filePath);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var fixName = Guid.NewGuid() + Path.GetExtension(fileName).ToLower();

            using (FileStream fs = System.IO.File.Create(Path.Combine(rootPath, fixName)))
            {
                await fileStream.CopyToAsync(fs);
                fs.Flush();
            }

            var fileObject = new BinaryObject(tenantId, Path.Combine(filePath, fixName), fileName);

            await _binaryObjectRepository.InsertAsync(fileObject);

            return fileObject;
        }

        public Task DeleteAsync(Guid id)
        {
            return _binaryObjectRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 根据 id 读取文件，没有读到时会抛出异常
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<byte[]> ReadAsync(Guid id)
        {
            var file = await _binaryObjectRepository.GetAsync(id);
            return await ReadAsync(file);
        }

        /// <summary>
        /// 根据 BinaryObject 读取文件，没有读到时会抛出异常
        /// </summary>
        public async Task<byte[]> ReadAsync(BinaryObject file)
        {
            var stream = GetReadStream(file);
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int) stream.Length);
            return buffer;
        }

        /// <summary>
        /// 根据 BinaryObject 获取文件流用于读取，没有读到时会抛出异常
        /// </summary>
        /// <exception cref="FileNotFoundException">没有找到文件</exception>
        public FileStream GetReadStream(BinaryObject file)
        {
            const int defaultBufferSize = 4096;

            var filePath = Path.Combine(_appFolders.UploadFolder, file.FilePath);
            var stream = new FileStream(filePath, FileMode.Open, 
                FileAccess.Read, FileShare.Read,
                defaultBufferSize, true);
            return stream;
        }

        /// <summary>
        /// 根据 id 获取文件流，没有读到时会抛出异常
        /// </summary>
        public async Task<FileStream> GetReadStreamAsync(Guid id)
        {
            var file = await _binaryObjectRepository.GetAsync(id);
            return GetReadStream(file);
        }
    }
}