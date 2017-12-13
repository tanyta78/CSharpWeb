namespace Intro.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
   
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime BirthDay { get; set; }

        public ICollection<StudentsCourses> CourseParticipateIn { get; set; } = new List<StudentsCourses>();

        public ICollection<Homework> Homeworks { get; set; }=new List<Homework>();
    }
}
