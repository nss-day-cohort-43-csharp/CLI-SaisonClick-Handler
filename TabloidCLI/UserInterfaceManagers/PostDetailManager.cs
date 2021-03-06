﻿using System;
using System.Collections.Generic;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    class PostDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private AuthorRepository _authorRepository;
        private PostRepository _postRepository;
        private TagRepository _tagRepository;
        private int _postId;

        public PostDetailManager(IUserInterfaceManager parentUI, string connectionString, int postId)
        {
            _parentUI = parentUI;
            _authorRepository = new AuthorRepository(connectionString);
            _postRepository = new PostRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);
            _postId = postId;
        }

        public IUserInterfaceManager Execute()
        {
            Post post = _postRepository.Get(_postId);
            Console.WriteLine("------------------------------");
            Console.WriteLine($"{post.Title} Details          ");
            Console.WriteLine("------------------------------");
            Console.WriteLine(" 1) View");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
            //Console.WriteLine(" 4) Note Management");
            Console.WriteLine(" 0) Go Back");
            Console.WriteLine("------------------------------");
            Console.WriteLine();

            Console.Write("> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    View();
                    return this;
                case "2":
                    Console.Clear();
                    AddTag();
                    return this;
                case "3":
                    Console.Clear();
                    RemoveTag();
                    return this;
                //case "4":
                //    Console.Clear();
                //    Edit();
                //    return this;
                case "0":
                    Console.Clear();
                    return _parentUI;
                default:
                    Console.WriteLine("Invlid Selection");
                    return this;
            }
        }


        private void View()
        {
            Post post = _postRepository.Get(_postId);
            List<Tag> tags = _postRepository.GetTagsByPost(_postId);
            Console.WriteLine("------------------------------");
            Console.WriteLine($"'{post.Title}' Details          ");
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Author: {post.Author.FirstName} {post.Author.LastName}");
            Console.WriteLine($"Blog: {post.Blog.Title}");
            Console.WriteLine($"Published: {post.PublishDateTime}");
            Console.WriteLine("Tags:");
            foreach (Tag tag in tags)
            {
                Console.Write("  -" + tag.Name);
            }


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to Post Menu");
            Console.ReadKey();
            Console.Clear();
        }


        private void AddTag()
        {
            Post post = _postRepository.Get(_postId);

            Console.WriteLine($"Which tag would you like to add to {post.Title}?");
            List<Tag> tags = _tagRepository.GetAll();

            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($"{i + 1}) {tag.Name}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                Tag tag = tags[choice - 1];
                _postRepository.InsertTag(post, tag);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection, no tags added");
            }
        }


        private void RemoveTag()
        {
            Post post = _postRepository.Get(_postId);
            Console.WriteLine($"Which tag would you liek to remove from {post.Title}?");
            List<Tag> tags = _postRepository.GetTagsByPost(_postId);
            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($" {i + 1}) {tag.Name}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                Tag tag = tags[choice - 1];
                _postRepository.DeleteTag(post.Id, tag.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection. Won't remove any tags.");
            }
        }

        
    }
}
