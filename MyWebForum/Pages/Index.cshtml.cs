﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyForum.Extensions;
using MyWebForum.Data;
using MyWebForum.Data.Interfaces;
using MyWebForum.Data.Repository.Repositories;

namespace MyWebForum.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyForumContext _db;
        private IPostRepository _posts;
        private ITopicRepository _topics;

        public bool IsAdmin { get; set; }

        public IEnumerable<MyWebForum.Models.Post> Posts { get; set; }
        public IEnumerable<MyWebForum.Models.Topic> Topics { get; set; }


        public IndexModel(MyForumContext db)
        {
            _db = db;
            _posts = new PostRepository(db);
            _topics = new TopicRepository(db);
        }

        public IActionResult OnGet()
        {
            if(HttpContext.Session.Get<MyWebForum.Models.User>("user") == null)
            {
                return RedirectToPage("/User/Login");
            }

            IsAdmin = HttpContext.Session.Get<MyWebForum.Models.User>("user").IsAdmin;
            Posts = _posts.GetPopularAllowedPosts(5);
            Topics = _topics.GetAllowedTopics();

            return Page();
        }

        public IActionResult OnPostSearchPost()
        {
            if(Request.Form["Search"].ToString() != null)
            {
                return RedirectToPage("/Post/Search", new { search = Request.Form["Search"].ToString() });
            }

            return RedirectToPage("Index");
        }
    }
}