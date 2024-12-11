namespace SubscribeExample
{
    internal class Content
    {
        public Content(string name)
        {
            Name = name;
        }

        public string Name { get; }
        /* 간소화된 접근자는 아래랑 기능은 같음
        public string Name
        {
            get => _name;
            private set
            {
                _name = value;
            }
        }
        private string _name; 
        */
    }
}
