
[![GitHub license](https://img.shields.io/github/license/dotnetcore/EasyCaching.svg)](https://github.com/dotnetcore/EasyCaching/blob/master/LICENSE)

----------

## Nuget包

| Package Name |  Version | Downloads
|--------------|  ------- | ----
| SuperShortLink.Core | ![](https://img.shields.io/badge/nuget-v1.2.1-blue) | ![](https://img.shields.io/badge/downloads-xM-brightgreen)|
| SuperShortLink.Api | ![](https://img.shields.io/badge/nuget-v1.2.1-blue) | ![](https://img.shields.io/badge/downloads-xM-brightgreen)|

---------

# `SuperShortLink`
> 这是一个基于.NET开源的短链生成及监控系统，它包含了在线生成短链、短链跳转长链、支持短链访问次数以及Web监控页面，可以帮助我们更容易地生成短链、监控短链！

-------

## 功能介绍
 - 基于.NET 6开发的后端及Web管理界面
 - 支持自定义短链长度
 - 支持在线短链生成及跳转长链
 - 支持实时统计短链访问次数
 - 支持多种持久化方式：MySQL/PostgreSQL/SqlServer(2012及以上)
 - 傻瓜式配置，开箱即用

------
## 构建项目

### Step 1 : 打开项目
通过VisualStudio打开 SuperShortLink.sln

### Step 2 : 配置数据库
- 可选：`MySQL`/`PostgreSQL`/`SqlServer`(2012及以上)
- 在`appsetting.json`文件中更新连接字符串
```
"ShortLink": {
    "Secrect": "vZCN8VhSge13UQrYjBTwKulWqsIOAocL0DkmRdxPMJf5tiHbn72z69aXpGyFE4",// 随机打乱的Base62编码
    "CodeLength": 6, //短链长度
    "DbType": "PostgreSQL", //DatabaseType:MySQL/PostgreSQL/SqlServer(仅支持SQL Server2012及以上)
    "ConnectionString": "Server=127.0.0.1;Port=5432;User Id=uid;Password=pwd;Database=test_db;",//数据库链接字符串
    "LoginAcount": "admin",   //登陆账号
    "LoginPassword": "123456" //登陆密码
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
);
CREATE TABLE short_link_appication (
    app_id      INT(11)        NOT NULL AUTO_INCREMENT,
    app_code    VARCHAR(255)   NOT NULL,
    app_name    VARCHAR(255)   NOT NULL,
    app_secret  VARCHAR(255)   NOT NULL,
    remark      VARCHAR(500)   NOT NULL,
    create_time TIMESTAMP(0)   NOT NULL,
    update_time TIMESTAMP(0)   NOT NULL,
    status      INT(11)        NOT NULL,
PRIMARY KEY (app_id) USING BTREE
);
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
CONSTRAINT pk_short_link PRIMARY KEY (id) 
);
CREATE TABLE short_link_appication (
    app_id      INT4           NOT NULL,
    app_code    VARCHAR(255)   NOT NULL,
    app_name    VARCHAR(255)   NOT NULL,
    app_secret  VARCHAR(255)   NOT NULL,
    remark      VARCHAR(500)   NOT NULL,
    create_time TIMESTAMP      NOT NULL,
    update_time TIMESTAMP      NOT NULL,
    status      INT4           NOT NULL,
CONSTRAINT pk_short_link_appication PRIMARY KEY (app_id) 
);

```
#### SQLServer

```
-- ----------------------------
-- SQLServer
-- ----------------------------
CREATE TABLE short_link (
    id           INT PRIMARY  KEY IDENTITY(1,1),
    short_url    VARCHAR(255) NOT NULL,
    origin_url   VARCHAR(255) NOT NULL,
    create_time  datetime2(0) NOT NULL,
    update_time  datetime2(0) NOT NULL,
    access_count INT NOT NULL 
);
CREATE TABLE short_link_appication (
    app_id      INT PRIMARY    KEY IDENTITY(1,1),
    app_code    VARCHAR(255)   NOT NULL,
    app_name    VARCHAR(255)   NOT NULL,
    app_secret  VARCHAR(255)   NOT NULL,
    remark      VARCHAR(500)   NOT NULL,
    create_time datetime2(0)   NOT NULL,
    update_time datetime2(0)   NOT NULL,
    status      INT            NOT NULL,
);
```

### Step 4 : 运行项目
- 登陆管理后台：{域名}/home/index
- 默认登陆账号密码：admin 123456
- 如需修改账号密码，更新`appsetting.json`的LoginAcount及LoginPassword即可

------------

## Web界面
![](media/web-login.png?raw=true)
![](media/web-list.png?raw=true)
![](media/web-generate.png?raw=true)


# 项目接入

## 1. 通过API扩展类库接入（推荐）

> API类库是基于HTTP请求，适合将接口开放给其他平台/系统调用，对应用屏蔽了Token，时间戳，应用Code等所需携带的请求细节

### 1. Step 1 : 安装包，通过Nuget安装包

```powershell
Install-Package Pandora.ShortLink.Api
```

### 2. Step 2 : 配置 Startup 启动类

```csharp
public class Startup
{
    //...
    
    public void ConfigureServices(IServiceCollection services)
    {
        //configuration
        services.AddShortLinkApi(option =>
        {
            option.ApiDomain = "短链服务域名";
            option.AppSecret = "应用秘钥";
            option.AppCode = "应用Code";
        });
    }    
}
```

### 3. Step 3 : IShortLinkApiService服务接口使用

```csharp
[Route("api/[controller]/[Action]")]
public class ShortLinkController : Controller
{
    private readonly IShortLinkApiService _apiService;
    public ShortLinkController(IShortLinkApiService apiService)
    {
        _apiService = apiService;
    }

    /// <summary>
    /// 解析生成短网址
    /// </summary>
    /// <param name="url">长链接</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Generate(string url)
    {
        var short_url = await _apiService.GenerateAsync(url);
        return short_url;
    }
}
```

## 2. 通过Core扩展类库接入

> Core类库是直连数据库的，适合由内部平台/系统调用，不需经过授权验证

### 1. Step 1 : 安装包，通过Nuget安装包

```
Install-Package Pandora.ShortLink.Core
```

### 2. Step 2 : 配置 Startup 启动类

```csharp
public class Startup
{
    //...
    
    public void ConfigureServices(IServiceCollection services)
    {
        //configuration
        services.AddShortLink(option =>
        {
            option.ConnectionString = "数据库链接";
            option.DbType = "数据库类型";//可选：DatabaseType.PostgreSQL/MySQL/SqlServer
            option.Secrect = "打乱后的Base62编码",
            option.CodeLength = "短链长度";
        });
    }    
}
```

### 3. Step 3 : IShortLinkService服务接口使用

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
    /// 解析生成短网址
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

## 短URL生成原理：混淆自增算法详解

- 标准Base64编码表如下：

![](media/content-base64.png?raw=true)
其中“+”和“/”在 URL 中会被编码为“%2B”以及“%2F”，需要进行再编码，因此直接使用标准 Base64 编码进行短URL 编码并不合适，所以，我们需要针对 URL 场景对 Base64 编码进行改造，Base64 编码表中的 62，63 进行编码移除，更新为Base62编码

### 混淆加密算法设计

![](media/content-encryption.png?raw=true)

1. 将标准编码随机打乱 ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789

> 举例：打乱成：s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKhaut

2. 6位长度标准编码与打乱后编码的对应关系
   | 计数器自增Id | 标准Base62编码 | 标准Base62编码（6位字符） | 打乱后Base62编码 | 打乱后Base62编码
   （6位字符） |
   | --- | --- | --- | --- | --- |
   | 6 | G | AAAAAG | y | sssssy |
   | 66 | BE | AAAABE | 9k | ssss9k |
   | 100 | Bm | AAAABm | 9E | ssss9E |

> 可以看出，虽然打乱了，但还顺序性还是很明显

3. 将前面补0再倒转，由于6位长度最大11位，为了避免倒转后超过该数值，因此补到10位
   | 计数器自增Id | 打乱后Base62编码
   （6位字符） | 前面补0到10位 | 倒转数字 | 倒转后的打乱Base62编码
   （6位字符） |
   | --- | --- | --- | --- | --- |
   | 6 | sssssy | 0000000006 | 6000000000 | yPFrgP |
   | 66 | ssss9k | 0000000066 | 6600000000 | 5xWCQH |
   | 100 | ssss9E | 0000000100 | 10000000 | ssSKph |

### 恢复混淆解密算法设计

![](media/content-decryption.png?raw=true)
将请求收到的短链Key根据打乱后的Base62编码转成十进制数，补0到10位，然后倒转就得到原来的短链Id

| 短链Key | 解析后的十进制数 | 前面补0到10位 | 倒转数字 |
| ------- | ---------------- | ------------- | -------- |
| yPFrgP  | 6000000000       | 6000000000    | 6        |
| 5xWCQH  | 6600000000       | 6600000000    | 66       |
| ssSKph  | 10000000         | 0010000000    | 100      |


---------
## 更多示例

1. 查看 [使用例子](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/src/SuperShortLink)
2. 查看 [测试用例](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/test/SuperShortLink.UnitTests)
3. 查看 [高并发解决方案](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/test/SuperShortLink.UnitTests)

