namespace SubscribeExample
{
    internal class Subscriber
    {
        public Subscriber(string name)
        {
            Name = name;
        }

        public string Name { get; }


        public void ContentUploadedCallback(Youtuber youtuber, Content content)
        {
            Console.WriteLine($"[{Name}] : {youtuber} uploaded content {content.Name}");
        }
    }
}
