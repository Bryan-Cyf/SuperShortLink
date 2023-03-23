
[![GitHub license](https://img.shields.io/github/license/dotnetcore/EasyCaching.svg)](https://github.com/dotnetcore/EasyCaching/blob/master/LICENSE)

----------

## Nuget包

| Package Name |  Version | Downloads
|--------------|  ------- | ----
| SuperShortLink.Core | ![](https://img.shields.io/badge/nuget-v1.1.0-blue) | ![](https://img.shields.io/badge/downloads-xM-brightgreen)|

---------

# `SuperShortLink`
> 是一个基于.NET开源的短链生成及监控系统，它包含了短链的生成、短链跳转长链、短链访问次数以及Web监控页面，可以帮助我们更容易地生成短链、监控短链！

-------

## 功能介绍
 - 基于.NET 6开发的后端及Web管理界面
 - 支持自定义短链长度
 - 支持在线短链生成及跳转长链
 - 统计短链访问次数
 - 支持多种持久化方式：MySQL/PostgreSQL/SqlServer(2012及以上)
 - 傻瓜式配置，开箱即用

------
## 构建项目

### Step 1 : 打开项目
打开 SuperShortLink.sln

### Step 2 : 配置数据库
可选：MySQL/PostgreSQL/SqlServer(2012及以上)
在`appsetting.json`文件中更新连接字符串
```
"ShortLink": {
    "Secrect": "s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut",// 62 位秘钥
    "CodeLength": 6, //短链长度
    "DbType": "PostgreSQL", //DatabaseType:MySQL/PostgreSQL/SqlServer(仅支持SQL Server 2012以上)
    "ConnectionString": "Server=127.0.0.1;Port=5432;User Id=uid;Password=pwd;Database=test_db;",//数据库链接字符串
    "LoginAcount": "admin",//登陆账号
    "LoginPassword": "123456"//登陆密码
}
```
### Step 3 : 执行数据库建表SQL

#### MySQL
```
-- ----------------------------
-- MySQL
-- ----------------------------
CREATE TABLE short_link (
    id            INT(11)      NOT NULL AUTO_INCREMENT,
    short_url     VARCHAR(255) NOT NULL,
    origin_url    VARCHAR(255) NOT NULL,
    create_time   TIMESTAMP(0) NOT NULL,
    update_time   TIMESTAMP(0) NOT NULL,
    access_count  INT(11)      NOT NULL,
PRIMARY KEY (id) USING BTREE 
)
```

#### PostgreSQL
```
-- ----------------------------
-- PostgreSQL
-- ----------------------------
CREATE TABLE short_link (
    id            SERIAL        NOT NULL,
    short_url     VARCHAR(128)  NOT NULL,
    origin_url    VARCHAR(128)  NOT NULL,
    create_time   TIMESTAMP     NOT NULL,
    update_time   TIMESTAMP     NOT NULL,
    access_count  INT4          NOT NULL
CONSTRAINT pk_short_link PRIMARY KEY ( id ) 
);

```
#### SQLServer

```
-- ----------------------------
-- SQLServer
-- ----------------------------
CREATE TABLE short_link (
    id           INT PRIMARY KEY IDENTITY(1,1),
    short_url    VARCHAR(255) NOT NULL,
    origin_url   VARCHAR(255) NOT NULL,
    create_time  datetime2(0) NOT NULL,
    update_time  datetime2(0) NOT NULL,
    access_count INT NOT NULL 
);
```

### Step 4 : F5运行项目

------------

## 扩展类库的基本使用

### Step 1 : 安装包

通过Nuget安装包
```
Install-Package SuperShortLink.Core
```

### Step 2 : 配置 Startup 启动类

下面提供一个简单的使用PostgreSQL,短链长度为6的Demo示例
```csharp
public class Startup
{
    //...
    
    public void ConfigureServices(IServiceCollection services)
    {
        //configuration
        services.AddShortLink(option =>
        {
            option.ConnectionString = "Server=127.0.0.1;Port=5432;User Id=uid;Password=pwd;Database=db;";
            option.DbType = DatabaseType.PostgreSQL;
            option.Secrect = "s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKut";
            option.CodeLength = 6;
        });
    }    
}
```

###  Step 3 : Controller接口定义 

```csharp
[Route("api/[controller]/[Action]")]
public class ShortLinkController : Controller
{
    private readonly IShortLinkService _shortLinkService;
    public ShortLinkController(IShortLinkService shortLinkService)
    {
        _shortLinkService = shortLinkService;
    }

    /// <summary>
    /// 解析生成短网址，并保存到数据库
    /// </summary>
    /// <param name="url">长链接</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Generate(string url)
    {
        var short_url = await _shortLinkService.GenerateAsync(url);
        return short_url;
    }
}
```

--------

## Web界面
![](media/web-login.png?raw=true)
![](media/web-list.png?raw=true)
![](media/web-generate.png?raw=true)


---------
## 更多示例

1. 查看 [使用例子](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/src/SuperShortLink)
2. 查看 [测试用例](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/test/SuperShortLink.UnitTests)

