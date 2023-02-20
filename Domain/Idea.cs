﻿using Domain.Base;

namespace Domain
{
    public class Idea : TenantAuditEntity<int>
    {
        public Idea()
        {

        }

        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsAnonymous { get; set; }
        public int DepartmentId { get; set; }
        public int AcademicYearId { get; set; }
        public int CategoryId { get; set; }

        public virtual AcademicYear AcademicYear { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Department Department { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();
    }
}
