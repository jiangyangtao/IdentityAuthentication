# IdentityAuthentication
身份验证

- 支持 JWT / Reference 两种 token
- 支持自定义多种数据源验证
- 支持自定义多种验证方式
- 支持 JWT 加密
- 支持 gRPC


## Generate RSA Key 生成 RSA 密钥对

``` C#

// Install nuget package: RSAExtensions

var rsa = RSA.Create();
var privateKey = rsa.ExportPrivateKey(RSAKeyType.Pkcs8);
var publicKey = rsa.ExportPublicKey(RSAKeyType.Pkcs8);

```