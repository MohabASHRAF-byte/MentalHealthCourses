using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly MentalHealthDbContext _dbContext;
        private readonly Domain.Repositories.IGenericContentServices<Article> _ContentServices;

        public ArticleRepository(MentalHealthDbContext dbContext, IGenericContentServices<Article> contentServices)
        {
            _dbContext = dbContext;
            _ContentServices = contentServices;
          
        }

       
            public async Task<Article> GetById(int ArticleId)

        {

            var SelectedArticle = _ContentServices.GetTableNoTracking()
            .Include(x => x.ArticleId).Where(x => x.ArticleId.Equals(ArticleId)).FirstOrDefault();


            return SelectedArticle;


            //await _dbContext.Set<Article>().FindAsync(Id);



        }


         public async Task<string> AddArticlAsync(Article articleMapper)
        {
            var CheckArticle = _ContentServices.GetTableNoTracking()
                    .Where(X => X.Content.Equals(articleMapper.Content))
                    .FirstOrDefaultAsync();
            if (CheckArticle != null)
            { return "The Content of This Meditation Already Exist"; }

            await _ContentServices.AddAsync(articleMapper);
            return "The Article has been Created Successfully!";

        }

        public async Task<string> DeleteArticlAsync(Article articleDeleted)
        {
            var Process = _ContentServices.BeginTransaction();
            try
            {
                await _ContentServices.DeleteAsync(articleDeleted);
                await Process.CommitAsync();
                return "Success Process!";
            }
            catch
            {
                await Process.RollbackAsync();
                return "failed Process!";
            }
        }
         public async Task<bool> IsExist(int IdOfArticle)
        {

            var ArticleCheck = _ContentServices
                .GetTableNoTracking()
            .Where(a => a.ArticleId.Equals(IdOfArticle)).FirstOrDefault();
            if (ArticleCheck is null)
            {
                return false;//the Article Not Exist Before 
            }
            return true;
        }

        public async Task<bool> IsExistDuringUpdate(string title, int Id)
        {

            var ArticleCheck = await _ContentServices
                .GetTableNoTracking()
                //To Do : Can't Assign same Values to Different Id's
                .Where(a => a.Title.Equals(title) & !a.ArticleId.Equals(Id))
                .FirstOrDefaultAsync();
            if (ArticleCheck is null)
            {
                return false;
                _ContentServices.UpdateAsync(ArticleCheck);
            }

            //Another Article Exists with same data
            return true;



        }

       public async Task<bool> IsExistByTitle(string title)
        {

        return await _ContentServices
               .GetTableNoTracking()
               .Where(a => a.Title.Equals(title))
               .FirstOrDefaultAsync() != null;



        }

        public async Task<bool> IsExistByContent(string content)
        {
            return await _ContentServices
                .GetTableNoTracking()
                .Where(a => a.Content.Equals(content))
                .FirstOrDefaultAsync() != null;




        }

        
        public async Task<string> UpdateArticlAsync(Article articles)
        {



            await _ContentServices.UpdateAsync(articles);
            return "The Article has been Updated Successfully!";


        }

        public async Task<(int, IEnumerable<Article>)> GetAllAsyncArticles(string? searchName, int pageNumber, int pageSize)
        {



            searchName ??= string.Empty;
            searchName = searchName.ToLower();
            var baseQuery = _dbContext.Articles
                .Where(r => r.Title.ToLower().Contains(searchName));
            var totalCount = await baseQuery.CountAsync();
            var articles = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return (totalCount, articles);











        }
    }
}









//*
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//
//*//