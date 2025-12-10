# Executar o projeto
API simples, criada com intuito de estudo das tecnologias dotnet (C#) e Entity Framework para ORM. Repositorio base [aqui](https://github.com/balta-io/2513)

- Necessario possui dotnet sdk 9.0 instalado na maquina
- Banco de dados postgres (Instalado ou via docker)

Para executar o banco de dados postgre via docker (ou podman) basta utilizar este comando:

    podman run --name postgres -e POSTGRES_PASSWORD=1q2w3e4r@#$ -p 5432:5432 -d postgres

Após execução do container basta acessar o banco de dados com o user e senha setados no comando na porta padrão 5432.

Dentro do projeto, há um arquivo sql chamado "Script.sql" para gerar as tabelas do banco de dados. Após executar o banco de dados, basta utilizar o script para gerar o banco, ou gerar o banco via migrations. (Abaixo possui os comando necessarios para gerar o banco via migrations).

# Fundamentos do Entity Framework

O Entity Framework (EF) é um Mapeador Objeto-Relacional (ORM) da Microsoft para .NET, que permite aos desenvolvedores trabalhar com bancos de dados usando objetos C# em vez de SQL puro, simplificando o acesso a dados, operações CRUD e a manutenção de aplicações, com o EF Core sendo a versão moderna, leve, de código aberto e multiplataforma. Ele abstrai as tabelas e colunas do banco de dados, mapeando-as para classes (entidades) e propriedades no código, facilitando a comunicação entre a aplicação e o banco de dados de forma mais orientada a objetos. 

### Tipos de mapeamentos

#### Fluent mapping
- Tecnica usada para configurar o mapeamento entre o código (classes e objetos) e o banco de dados (tabelas e colunas), permitindo um código mais limpo, legível e conciso através do encadeamento de métodos


#### Data annotations
- Feitos diretamente nas classes,
- Mais simples e diretos
- Atributos (como [Required], [Range], [StringLength]) que você "decora" suas classes e propriedades para aplicar regras de validação e metadados.

No Entity Framework, tanto o uso de Data Annotations quanto de Fluent Mapping pode ser válido, e a escolha depende do nível de complexidade do modelo e da flexibilidade necessária.

**Data Annotations** são mais simples e permitem configurar diretamente na classe atributos como Required, MaxLength, Key, entre outros. Elas funcionam bem quando o modelo é relativamente simples e as convenções atendem à maior parte das necessidades.

**Fluent Mapping**, por outro lado, oferece controle completo sobre o mapeamento. É a abordagem indicada quando precisamos configurar relacionamentos complexos, chaves compostas, índices, comportamento de cascade, tipos de coluna específicos ou qualquer configuração avançada que não seja possível via Data Annotations.

Em resumo:

Modelos simples → Data Annotations são suficientes e deixam o código mais limpo.

Modelos complexos ou altamente customizados → Fluent Mapping é a melhor opção, pois oferece maior flexibilidade e precisão no mapeamento.

Em muitos projetos, inclusive grandes, é comum utilizar as duas abordagens combinadas, aplicando Data Annotations para regras simples e deixando o Fluent Mapping para configurações avançadas.


# Migrations

Migrations (ou migrações) são o mecanismo do Entity Framework para controlar a evolução do banco de dados conforme o seu modelo de classes muda.
Elas permitem:

- Criar o banco a partir do modelo
- Atualizar o banco quando as entidades mudam
- Versionar alterações de esquema
- Manter histórico das mudanças

Ou seja, migrations sincronizam o modelo de domínio com o banco de dados sem precisar escrever SQL manualmente.

Para criar uma nova migração, devemos utilizar o comado:

    dotnet ef migrations add NomeDaMigration
    
Esse comando gera um arquivo de migration contendo as alterações detectadas no modelo. Esse arquivo migrations fica dentro do diretorio "Migrations" no projeto dotnet.
Após gerar a migration, podemos aplicar ela ao nosso banco de dados com o seguinte comando:

    dotnet ef database update

### Método AsNoTracking()

Por padrão, quando você faz uma consulta como:

    var usuarios = context.Usuarios.ToList();

O EF rastrea cada objeto retornado.
Isso significa que:

- Ele guarda o estado original da entidade
- Controla mudanças (Change Tracker)
- Permite atualizar automaticamente quando você chama SaveChanges()

Quando você adiciona AsNoTracking(), o EF não rastreia as entidades.
Ou seja, os objetos retornados:

- Não são monitorados para alterações
- Não podem ser atualizados automaticamente com SaveChanges()
- Ocupam menos memória
- Tornam a consulta mais rápida
- É ideal para cenários de somente leitura.

Exemplo de uso:

    var usuarios = context.Usuarios
    .AsNoTracking()
    .Where(u => u.Ativo)
    .ToList();

