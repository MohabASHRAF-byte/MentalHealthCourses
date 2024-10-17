using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IArticleRepository
    {
        public Task<string> AddArticlAsync(Article ArticleMapper);
        public Task<string> DeleteArticlAsync(Article article);
        public Task<(int , IEnumerable<Article>)> GetAllAsyncArticles(string? search, int requestPageNumber, int requestPageSize);
        public Task<Article> GetById(int ArticleId);
        public Task<string> UpdateArticlAsync(Article articles);
        public Task<bool> IsExist(int IdOfArticle);
        public Task<bool> IsExistDuringUpdate(string title, int Id);
        public Task<bool> IsExistByTitle(string title);
        public Task<bool> IsExistByContent(string content);


    }
}
