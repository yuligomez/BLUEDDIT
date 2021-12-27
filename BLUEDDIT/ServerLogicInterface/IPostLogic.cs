using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogicInterface
{
    public interface IPostLogic
    {
        Response PostPost(Post post, string themeName);
        Response DeletePost(string namePost);
        Response AssociatePostToTheme(string namePost, string nameTheme);
        Response ModifyPost(string oldPostName, Post newPost);
        Response DissassosiatePostToTheme(string namePost, string nameTheme);
        Post GetPostByName(string name);
        List<Post> GetPostsByOrder();
        List<File> GetFilteredFiles(Post post, FileFilter filter);
    }
}
