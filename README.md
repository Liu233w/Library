# Library Manage System (图书管理系统)

[![Build Status](https://travis-ci.com/Liu233w/Library.svg?token=at1dz5LtwtReX6e4EKDg&branch=master)](https://travis-ci.com/Liu233w/Library)

The project is hosted on [GitHub](https://github.com/Liu233w/Library). I need to add you to the project or you can't see it as a result of private repo.

## Developer
- Liu Shumin 2015303087
- Cheng Luyao 2015303204
- Zeng Yao 2015303219
- Wang Siyuan 2015303275
- XiaoJing 2015303251
- Cui Fengli 2015303124
- Tang Yuying 2015303250
- YuGuanqing 2015303202
- Deng Xiongfei 2015303205
- Gao Chenyu 2015303199

## Requirement
- Windows or Linux system
- Sql Server (Or Sql Server Express) 2012+
- DotNet Core Runtime 2.0+ (If you want to compile the repo yourself, you also need to install DotNet Core SDK)
- Nodejs 7+ (please add the path of the executable to your system variable `path`)
- If you use Windows system, you may need IIS

## Deployment

### Get the migraton and dist file

You can get the files by:

1. Go to our [release page](https://github.com/Liu233w/Library/releases) and download dist.zip and migrator.zip

or

2. Open repo folder and run the script `build/build.sh` to build binary file and `build/build-migrator.sh` to build migrator. The `cmd` script is for windows users.

### Deploy database
1. Extract the `migrator.zip` or go to the `migrator` folder in the repo (if you compile the binary file yourself).
2. Edit the connection string in `appsetting.json`.
3. Execute script file `deploy-database.sh` in the folder or just execute the command `dotnet Library.Migrator` in the folder
4. Follow the instruction in the program
5. If you can't use migrator, you can deploy database by sql script. The script file is at `sqls` folder in the repo. Or sqls.zip in the release page.

### Deploy server
1. Extract the `dist.zip` or go to the `dist` folder
2. Edit the connection string in `appsetting.json`.
3. If you use the windows server and iis, just add a new website with the dist folder as content folder, just as you did before.
4. If you use Linux, you can execute `dotnet Library.Web.Mvc` in `dist` folder. The program will open a port on `localhost:5000`. You can use nginx or other reverse proxy program as you like.
