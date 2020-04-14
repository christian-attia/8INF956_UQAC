# 8INF956 - Plugins

## Questions
### Comment organisez votre solution? (Combien de projets? Quels types de projets ?)

Il y a 2 projets + autant de projets que de plugins (situés dans le dossier `./plugins`) :
- `AppWithPlugin`: l'application prinipale. Elle charge tous les plugins du dossier `plugins` et affiche un sample des utilsateurs chargés pour chacun de ces plugins
- `PluginBase`: le projet qui définit l'interface des plugins `IUsersPlugin` ainsi que la classe `User`
- `plugins/*`: contiens tous les plugins de chargement d'utilisateurs


### Dans quel projet mettez-vous la class `User`? 
Dans le projet `PluginBase`, c'est le projet qui fait le lien entre l'application principale et les plugins, donc il contient l'interface que les plugins doivent implémenter et la classe `User`.


### En vous basant sur la documentation, à quoi ressemble l’interface d’un plugin dans notre système ? 
Nos plugins ont pour seule fonction le chargement d'utilisateur, l'interface choisie :

```csharp
public interface IUsersPlugin
{
    string Name { get; }
    string Description { get; }
    IEnumerable<User> LoadUsers();
}
```

## Utilisation

### Build
```
dotnet build Plugins.sln
```

### Run
**À la racine de la solution:**
```
dotnet run --project AppWithPlugin/AppWithPlugin.csproj
```
Sinon, si vous utilisez vscode, le fichier `launch.json` est déjà correctement configuré, plus qu'à lancer avec F5.


### Ajout d'un nouveau plugin
Pour ajouter un nouveau plugin, par exemple `UsersInMemoryPlugin` :
1. Créer le projet: `dotnet new classlib -o plugins/UsersInMemoryPlugin`
2. Ajouter le projet à la solution: `dotnet sln add plugins/UsersInMemoryPlugin/UsersInMemoryPlugin.csproj`
3. Ajouter la référence du projet `PluginBase` à ce projet (dans le `.csproj`) pour avoir accès à l'interface et à la classe `User` :
```
<ItemGroup>
    <ProjectReference Include="..\..\PluginBase\PluginBase.csproj">
        <Private>false</Private>
        <ExcludeAssets>runtime</ExcludeAssets>
    </ProjectReference>
</ItemGroup>
```
4. Créer la classe principale du plugin qui doit étendre l'interface `IUserPlugin`, par exemple:
```csharp
public class UsersMemoryLoader : IUsersPlugin
{
    public string Name { get => "UsersInMemoryPlugin"; }
    public string Description { get => "Load fake users"; }
    public IEnumerable<User> LoadUsers() => new List<User>{
            new User("John", "Doe", "john@doe.com"),
            new User("Terry", "Alvarez", "terry@alvarez.com"),
            new User("James", "Collin", "james@collin.com")
        };
}
```

Maintenant, suivez les instructions du début pour *build* et *run* la solution. Au lancement de l'application, le plugin sera automatiquement chargé et exécuté par le programme (pour chaque plugin un sample des utilisateurs chargés sera affiché).

Résultat: 
```
Plugin: UsersInMemoryPlugin
        Sample of the users loaded:
John Doe - john@doe.com
Terry Alvarez - terry@alvarez.com
James Collin - james@collin.com

Plugin: UserJSONLoader
        Sample of the users loaded:
Data source: ./users/users_default.json
Data source: ./users/users_mock.json
Hube Worland - hworland0@t.co
Prinz Roelofs - proelofs1@behance.net
Sascha Looney - slooney2@sbwire.com
Haslett Beardsley - hbeardsley3@netvibes.com
Wilfrid Stanislaw - wstanislaw4@privacy.gov.au
Eddy Brombell - ebrombell5@1und1.de
Aloysia Feron - aferon6@illinois.edu
Lennie Dzeniskevich - ldzeniskevich7@vkontakte.ru
Marv Hinze - mhinze8@guardian.co.uk
Dionysus Jelkes - djelkes9@hhs.gov
```
