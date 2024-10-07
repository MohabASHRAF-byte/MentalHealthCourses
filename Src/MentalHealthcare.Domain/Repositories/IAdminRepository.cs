using MentalHealthcare.Domain.Entities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IAdminRepository<T>  where T : Admin
    {

        #region Signature_CRUD_Operation at Dashboad

        public ICollection<User> GetUsers();
        public User GetById(int id);
        public User GetByUserName(string userNames);
        public int DeleteUser(User users);



        public int AddVideos(Video videos); //Upload
        public int UpdateVideos(Video videos);
        public int DeleteVideos(Video videos);


        public int AddArticle(Article articles); //Upload
        public int UpdateArticle(Article articles);
        public int DeleteArticle(Article articles);


        public int AddMeditation(Meditation meditations); //Upload
        public int UpdateMeditation(Meditation meditations);
        public int DeleteMeditation(Meditation meditations);


        public int AddPodcast(Podcast podcasts); //Upload
        public int UpdatePodcast(Podcast podcasts);
        public int DeletePodcast(Podcast podcasts);



        public int AddCourses(Course courses);//Upload
        public int UpdateCourses(Course courses);
        public int DeleteCourses(Course courses);



        public int AddPdf(Pdf Pdf); //Upload
        public int DeletePdf(Pdf Pdfs);

        #endregion







    }
}
