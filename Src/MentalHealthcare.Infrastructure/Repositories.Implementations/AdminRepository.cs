using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.implementations
{
    public interface AdminRepository<T> : IAdminRepository<T> where T : Admin
    {


  private readonly MentalHealthDbContext _dbContext;

  public AdminRepository(MentalHealthDbContext dbContext)
  {
      _dbContext = dbContext;
  }

  #region Add & Create Content
  public async Task AddArticle(Article articles)
  {
      await _dbContext.articles.AddAsync(articles);

      await _dbContext.SaveChangesAsync();
  }


  public async Task AddMeditation(Meditation meditations)
  {
      await _dbContext.meditations.AddAsync(meditations);
      await _dbContext.SaveChangesAsync();

  }

  public async Task AddPdf(Pdf Pdf)
  {
      await _dbContext.pdfs.AddAsync(Pdf);
      await _dbContext.SaveChangesAsync();
  }

  public async Task AddPodcast(Podcast podcasts)
  {
      await _dbContext.podcasts.AddAsync(podcasts);
      await _dbContext.SaveChangesAsync();

  }

  // Dependent on Bunny.net
  public Task AddVideos(Video videos)
  {
      throw new NotImplementedException();
  }

  public Task AddCourses(Course courses)
  {
      throw new NotImplementedException();
  }
  #endregion

  #region Delete Content


  public int DeleteArticle(Article articles)
  {

      _dbContext.articles.Remove(articles);

      return (_dbContext.SaveChanges());


  }
  public int DeleteMeditation(Meditation meditations)
  {
      _dbContext.meditations.Update(meditations);

      return (_dbContext.SaveChanges());
  }

  public int DeletePdf(Pdf Pdfs)
  {
      _dbContext.pdfs.Remove(Pdfs);

      return (_dbContext.SaveChanges());
  }

  public int DeletePodcast(Podcast podcasts)
  {

      _dbContext.podcasts.Remove(podcasts);

      return (_dbContext.SaveChanges());

  }




  // Dependent on Bunny.net

  public int DeleteCourses(Course courses)
  {
      throw new NotImplementedException();
  }



  public int DeleteVideos(Video videos)
  {
      throw new NotImplementedException();
  }




  #endregion

  #region UpDate Content
  public async Task UpdateArticle(Article articles)
  {

      _dbContext.articles.Update(articles);

      await _dbContext.SaveChangesAsync();


  }
  public async Task UpdateMeditation(Meditation meditations)
  {

      _dbContext.meditations.Update(meditations);
      await _dbContext.SaveChangesAsync();

  }

  public async Task UpdatePodcast(Podcast podcasts)
  {
      _dbContext.podcasts.Update(podcasts);

      await _dbContext.SaveChangesAsync();

  }

  // Dependent on Bunny.net

  public Task UpdateCourses(Course courses)
  {
      throw new NotImplementedException();
  }
  public Task UpdateVideos(Video videos)
  {
      throw new NotImplementedException();
  }
  #endregion

  #region Admin & User
  public async Task<IEnumerable<User?>> GetAllAsync()
   => await _dbContext.Set<User>().ToListAsync();

  public async Task<User?> GetByIdAsync(int id)
    => await _dbContext.Set<User>().FindAsync(id);


  public async Task<User?> GetByUserName(string userNames)
      => await _dbContext.Set<User>().FindAsync(userNames);

  public int DeleteUser(User users)
  {

      _dbContext.users.Remove(users);

      return (_dbContext.SaveChanges());
  }

  #endregion






    }
}
