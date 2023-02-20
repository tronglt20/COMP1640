﻿namespace Domain
{
    public class UserDepartment
    {
        public UserDepartment()
        {
        }

        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        
        public virtual User User { get; set; }
        public virtual Department Department { get; set; }
    }
}
