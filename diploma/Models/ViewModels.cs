using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace diploma.Models
{
    public class ViewModels
    {

    }
    public class CourseCreateViewModel
    {
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Название курса")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Категория курса")]
        public string Cathegory { get; set; }
        [Required]
        [Display(Name = "Цена курса")]
        public int Price { get; set; }
        [Required]
        [Display(Name = "Логотип курса")]
        public HttpPostedFileBase img { get; set; }
    }

    public class TestReport1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AnswersReport
    {
        public string answerText { get; set; }
        public bool answer { get; set; }
        public bool userAnswer { get; set; }
    }
    public class lessonsReport
    {
        public string QuestionText { get; set; }
        public List<AnswersReport> answersReports { get; set; }
    }
    public class DiagramViewModel
    {
        public string Lesson { get; set; }
        public int Mark { get; set; }
    }

}