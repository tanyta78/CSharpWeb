﻿namespace Intro.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? ManagerId { get; set; }

        public Employee Manager { get; set; }
        
        public ICollection<Employee> Subordinates { get; set; }=new List<Employee>();
}
}
