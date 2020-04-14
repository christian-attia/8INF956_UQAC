namespace PluginBase
{
  public class User {

    public User(string firstName, string lastName, string email) {
      this.FirstName = firstName;
      this.LastName = lastName;
      this.Email = email;
    }
    public string FirstName;
    public string LastName;
    public string Email;

    public override string ToString() {
      return $"{FirstName} {LastName} - {Email}";
    }
  }
}