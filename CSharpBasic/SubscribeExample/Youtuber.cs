namespace SubscribeExample
{
    internal class Youtuber
    {
        public Youtuber() 
        {
            Contents = new List<Content>(4);
        }


        public List<Content> Contents { get; private set; }

        // 컨텐츠가 업로드되었을때 실행해야할 함수 목록이 필요
        public delegate void ContentUploadedHandler(Youtuber youtuber, Content content);
        // event 한정자 : 외부에서는 이 대리자에 접근할때 += 과 -= 연산만 가능하게 제한. (대리자의 구독/구독취소 기능 외 캡슐화)
        // public 한 멤버 대리자를 정의했는데, event 가 안붙는 형태다? -> 뭔가 설계 이상할 가능성 99% (구독자가 유튜버의 모든 구독자에게 알림을 임의로 할수있도록 하는행위와 같다)
        public event ContentUploadedHandler OnContentUploaded; // 함수참조 목록을 가지고있는 객체 (대리자 객체) - 컨텐츠 업로드 알림을 받고싶은 구독자들이 여기다가 함수 등록


        public void UploadContent(Content content)
        {
            Contents.Add(content);
            OnContentUploaded?.Invoke(this, content);
            //OnContentUploaded.Invoke(this, content); // 왠만하면 이형태 쓰는게 가독성이 좋다 (제 3자가 봤을때 정의를 픽업하지않고도 대리자라는 사실을 알기편하다.)
            //OnContentUploaded(this, content);
        }
    }
}
