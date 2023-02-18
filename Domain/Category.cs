﻿using Domain.Base;

namespace Domain
{
    public class Category : TenantEntity<int>
    {
        public Category()
        {

        }

        public string Name { get; set; }

        public virtual ICollection<Idea> Ideas { get; set; } = new HashSet<Idea>();
    }
}
