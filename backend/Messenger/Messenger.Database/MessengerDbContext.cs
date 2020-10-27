using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Database
{
    public class MessengerDbContext : DbContext
    {
        public MessengerDbContext(DbContextOptions<MessengerDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<UserConversation> UserConversations { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conversation>()
                .HasOne(p => p.Creator)
                .WithMany(b => b.ConversationsCreated)
                .HasForeignKey(a => a.CreatorId);

            modelBuilder.Entity<UserConversation>()
                .HasOne(p => p.Conversation)
                .WithMany(b => b.Conversations)
                .HasForeignKey(a => a.ConversationId);

            modelBuilder.Entity<UserConversation>()
                .HasOne(p => p.User)
                .WithMany(b => b.Conversations)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Message>()
                .HasOne(p => p.Conversation)
                .WithMany(b => b.Messages)
                .HasForeignKey(a => a.ConversationId);

            modelBuilder.Entity<Message>()
                .HasOne(p => p.Sender)
                .WithMany(b => b.MessagesSended)
                .HasForeignKey(a => a.SenderId);

            modelBuilder.Entity<UserRelation>()
                .HasOne(p => p.Friend)
                .WithMany(b => b.Friends)
                .HasForeignKey(a => a.FriendId);

            modelBuilder.Entity<UserRelation>()
                .HasOne(p => p.User)
                .WithMany(b => b.FriendsAdded)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Token>()
                .HasOne(p => p.User)
                .WithMany(b => b.Tokens)
                .HasForeignKey(a => a.UserId);
        }
    }
}
