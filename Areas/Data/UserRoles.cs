namespace AsterWebApp.Areas.Identity.Data
{
    public class UserRoles
    {
        public const string SemisAdmin = "SemisAdmin";       
        public const string SchoolAdmin = "SchoolAdmin";
        public const string Teacher = "Teacher";
        public const string Parents = "Parents";
        public const string Student = "Student";
        public const string Staff = "Staff";
        public const string User = "User";
        public const string ClassUser = "ClassUser";
    }

    public class UserTypes
    {
        public const int SemisAdmin = 1;

        public const int SchoolAdmin = 10;
        public const int Teacher = 100;
        public const int Parents = 300;
        public const int Student = 400;
        public const int Staff = 200;
        public const int User = 500;
        public const int ClassUser = 600;
    }
}
