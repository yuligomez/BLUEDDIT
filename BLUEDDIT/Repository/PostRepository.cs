using Domain;
using ServerRepositoryInterface;
using System.Collections.Generic;
using System.Linq;

namespace ServerRepository
{
    public class PostRepository : IPostRepository
    {
        private static PostRepository Instance = null;
        private static readonly object ObjectLock = new object();
        private List<Post> Posts;

        private PostRepository() 
        {
            this.Posts = new List<Post>();
        }

        public static PostRepository GetInstance()
        {
            if (PostRepository.Instance == null)
            {
                lock (PostRepository.ObjectLock)
                {
                    if (PostRepository.Instance == null)
                    {
                        PostRepository.Instance = new PostRepository();
                    }
                }
            }
            return PostRepository.Instance;
        }
        public void DeletePost(Post post)
        {
            this.Posts.Remove(post);
        }

        public Post GetPostByName(string name)
        {
            Post post = null;
            foreach (Post postDB in Posts)
            {
                if (postDB.Name == name)
                {
                    post = postDB;
                }
            }
            return post;
        }

        public void AddPost(Post post)
        {
            this.Posts.Add(post);
        }

        public List<Post> GetPostsByOrder()
        {
            IEnumerable<Post> posts = Posts.OrderBy(post => post.CreationDate);
            return posts.ToList();
        }
    }
}