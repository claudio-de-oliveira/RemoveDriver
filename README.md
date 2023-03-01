# Driver Remoto

Api simulando um driver remoto com operações básicas para a manutenção de pastas e arquivos. Os aplicativos clientes podem, por exemplo, criar e remover pastas, fazer o upload ou download de arquivos etc.

Para instalar o driver no Docker, no PowerShell vá para a pasta onde se encontra o arquivo ``docker-compose.yml`` (abaixo) e execute ``docker-compose up -d``.

```
version: '3.4'

services:
    driverserver:
        image: remotedriver
        build: 
            context: .
            dockerfile: DriverApi\Dockerfile
        ports: 
            - "5253:80" 
        environment:
            Root: "/root/arquivos"
            ASPNETCORE_URLS: http://+:80
            ASPNETCORE_ENVIRONMENT: Development
        volumes:
            - ${USERPROFILE}\Driver:/root/arquivos             # folder em localhost: (c:\users\<...>\Driver)
            - ${USERPROFILE}\.aspnet\https:/root/.aspnet/https:ro
            - ${USERPROFILE}\.aspnet\DataProtection-Keys\:/root/.aspnet/DataProtection-Keys
            - ${APPDATA}\Microsoft\UserSecrets:/var/opt/mssql/secrets
```

Você pode se basear na interface ``IDriverService`` e na classe ``DriverService`` 
(ver Tests) para implementar os serviços nos clientes.

## Testes

Os testes foram separados em testes da camada de Infraestrutura (``InfrastructureTier_TestClass``), 
camada de aplicação (``AplicationTier_TestClass``) e a camada de apresentação (``PresentationTier_TestClass``) - 
esta última contendo a própria API. NOTE QUE OS TESTES NÃO ESTÃO COMPLETOS.


## Contato

Email: clalulana@gmail.com

Whatsapp: (41) 98736-0222
