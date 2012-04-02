

namespace Retail.Models
{
    public class Employee:User
    {
        public DepartmentType Dept { get; set; }
                
        public override UserType UserType
        {
            get
            {
                return UserType.Employee;
            }
        }


    }

    public enum DepartmentType
    {
        Admin,
        Finance,
        IT
    }
}