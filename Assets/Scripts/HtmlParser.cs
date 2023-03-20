using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HtmlAgilityPack;
using UnityEngine.UI;

public class HtmlParser : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _submitButton;
    [SerializeField] private ResourceImage _imageTempleate;
    [SerializeField] private RectTransform _grid;
    
    private readonly ContentLoader _contentLoader = new ContentLoader();

    private Dictionary<string, Texture2D> cacheImages = new Dictionary<string, Texture2D>();
    private List<ResourceImage> _images = new List<ResourceImage>();

    private void Awake()
    {
        _inputField.onSubmit.AddListener(LoadAndParse);
        _submitButton.onClick.AddListener(() => LoadAndParse(_inputField.text));
    }
    
    private void LoadAndParse(string url)
    {
        if (string.IsNullOrEmpty(url)) return;
        StartCoroutine(_contentLoader.GetRequest(url, 
            data => StartCoroutine(ParsePage(data))));
    }

    private IEnumerator ParsePage(string data)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(data);
        HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes("//img");
        SpawnImages(imgNodes.Count);

        int index = 0;
        foreach (HtmlNode imgNode in imgNodes)
        {
            string imgUrl = imgNode.GetAttributeValue("src", "");

            if (cacheImages.ContainsKey(imgUrl))
                _images[index].Construct(cacheImages[imgUrl]);
            else
                yield return StartCoroutine(
                    _contentLoader.LoadImage(imgUrl, _images[index].Construct));
            index++;
        }
    }

    private void SpawnImages(int count)
    {
        ClearImages();

        for (int i = 0; i < count; i++)
        {
            var image = Instantiate(_imageTempleate, _grid);
            image.gameObject.SetActive(true);
            _images.Add(image);
        }
    }

    private void ClearImages()
    {
        foreach (var image in _images)
        {
            image.Deinitialize();
            Destroy(image.gameObject);
        }
        
        _images.Clear();
    }
}

//
// Тестове завдання на позицію Unity developer:
// Парсер картинок з будь якого вебсайту
// Екрани:
// 1. Екран пустий з полем вводу сайту і кнопка зпарсити
// 2. Екран результат список картинок які були на цьому сайті
//
// Механізм:
// Вводимо сайт в поле, натискаємо кнопку і починає йти завантаження, відображається  відразу кількість плиток які дорівнюють кількості картинок на сайті.
// Потім, коли сірі картинки заспавнились - завантажуються  самі картинки в ці квадрати.
// Особливість: картинки повинні кешуватись, якщо ми захочемо, ще раз ввести Гугл.ком, нам повинно відображати відразу картинки без завантаження з інтернету тому що, вони вже закешовані. Але, якщо на сайті картинки оновляться, то й тут відповідно також.
//
//     Термін виконання: 2 години