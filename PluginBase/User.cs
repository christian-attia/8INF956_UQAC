namespace PluginBase
{
  public class User {
    public string FirstName;
    public string LastName;
    public string Email;

    public override string ToString() {
      return $"{FirstName} {LastName} - {Email}";
    }
  }
}