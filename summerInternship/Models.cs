namespace summerIntership;
public class UserWithRoles
{
    public int id { get; set; }
    public string username { get; set; }
    public string[] role_names { get; set; }
}

public class RoleStatistic
{
    public int id { get; set; }
    public string name { get; set; }
    public int user_count { get; set; }
}
