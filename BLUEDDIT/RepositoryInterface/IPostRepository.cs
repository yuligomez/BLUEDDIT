using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerRepositoryInterface
{
    public interface IPostRepository
    {
        void DeletePost(Post post);
        void AddPost(Post post);
        Post GetPostByName(string name);
        List<Post> GetPostsByOrder();
    }
}
