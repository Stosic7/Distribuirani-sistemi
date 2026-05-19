import java.io.Serializable;

public class Message implements Serializable {
    private String title;
    private String content;

    public Message(String title, String content) {
        this.title = title;
        this.content = content;
    }

    public String getTitle() {
        return title;
    }

    public String getContent() {
        return content;
    }

    public String toString() {
        return "Naslov: " + title + ", Sadrzaj: " + content;
    }
}
