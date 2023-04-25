
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
- [x] 基于.NET 6开发的后端及Web管理界面
- [x] 支持自定义短链长度
- [x] 支持在线短链生成及跳转长链
- [x] 支持实时统计短链访问次数
- [x] 支持多种持久化方式：MySQL/PostgreSQL/SqlServer(2012及以上)
- [x] 傻瓜式配置，开箱即用

------
## 文档
---------

- 项目的运行及接入，看这个文档
### [项目运行及接入文档](https://chenyuefeng.blog.csdn.net/article/details/130222045)

---------

- 项目的整体架构设计，加解密混淆算法原理，看这个文档
### [项目架构设计及算法详解文档](https://chenyuefeng.blog.csdn.net/article/details/130194794)

----------

## Web界面
- 登录页

![](media/web-login.png?raw=true)
- 短链列表页

![](media/web-list.png?raw=true)
- 短链生成页

![](media/web-generate.png?raw=true)
- 应用列表页

![](media/web-application-create.png?raw=true)
![](media/web-application-list.png?raw=true)
![](media/web-application-dashboard.png?raw=true)

-----------

## 更多示例

1. 查看 [使用例子](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/src)
2. 查看 [测试用例](https://github.com/Bryan-Cyf/SuperShortLink/tree/master/test)

----------
## 短URL生成原理：混淆自增算法详解

- 标准Base64编码表如下：

![](media/content-base64.png?raw=true)


其中“+”和“/”在 URL 中会被编码为“%2B”以及“%2F”，需要进行再编码，因此直接使用标准 Base64 编码进行短URL 编码并不合适，所以，我们需要针对 URL 场景对 Base64 编码进行改造，Base64 编码表中的 62，63 进行编码移除，更新为Base62编码

### 混淆加密算法设计

![](media/content-encryption.png?raw=true)

1. 将标准编码随机打乱 ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789

> 举例：打乱成：s9LFkgy5RovixI1aOf8UhdY3r4DMplQZJXPqebE0WSjBn7wVzmN2Gc6THCAKhaut

2. 6位长度标准编码与打乱后编码的对应关系
   | 计数器自增Id | 标准Base62编码 | 标准Base62编码（6位字符） | 打乱后Base62编码 | 打乱后Base62编码（6位字符） |
   | --- | --- | --- | --- | --- |
   | 6 | G | AAAAAG | y | sssssy |
   | 66 | BE | AAAABE | 9k | ssss9k |
   | 100 | Bm | AAAABm | 9E | ssss9E |

> 可以看出，虽然打乱了，但还顺序性还是很明显

3. 将前面补0再倒转，由于6位长度最大11位，为了避免倒转后超过该数值，因此补到10位

   | 计数器自增Id | 打乱后Base62编码（6位字符） | 前面补0到10位 | 倒转数字 | 倒转后的打乱Base62编码（6位字符） |
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
