业务逻辑层
放置具体业务所使用的DTO类，DTO参数验证类、DTO到实体，实体到DTO映射配置类、业务接口类、业务接口实现类
验证器使用FluentValidation:
https://docs.fluentvalidation.net/en/latest/
https://github.com/FluentValidation/FluentValidation
DTO到实体，实体到DTO的转换建议使用AutoMapper，实体到DTO数据装载建议提取成方法，方法名以Prepare+DTO类名命名比如UserDto 同步方法名为PrepareUserDto 异步方法名PrepareUserDtoAsync：
https://automapper.org/
https://github.com/AutoMapper/AutoMapper

