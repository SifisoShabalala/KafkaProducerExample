using System;

namespace KafkaProducerExample
{
    public class GitHubMessage
    {
        public GitHubMessage(GitHubPushModel model)
        {
            Author = model.pusher.name;
            RepositoryName = model.repository.name;
            UpdatedAt = model.repository.updated_at;
        }

        public string Author { get; }
        public string RepositoryName { get; }
        public DateTime UpdatedAt { get; set; }
    }
}
