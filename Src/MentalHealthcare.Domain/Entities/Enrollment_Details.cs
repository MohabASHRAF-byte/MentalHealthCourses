using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MentalHealthcare.Domain.Entities.Courses;

namespace MentalHealthcare.Domain.Entities
{
    public class EnrollmentDetails
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(200)]
        public string Progress { get; set; } = default!;

        // Foreign Key to SystemUser
        public int SystemUserId { get; set; }
        public SystemUser SystemUser { get; set; } = default!;

        // Foreign Key to Course
        public int CourseId { get; set; }
        public Course Course { get; set; } = default!;
    }
}