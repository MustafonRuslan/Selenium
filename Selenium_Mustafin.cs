using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;

namespace Selenium_Mustafin;

public class Selenium_Test
{
    public ChromeDriver driver;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--disable-extensions"); 
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        Authorization();
    }

    [Test] //Тест №1. Авторизация
    public void Authorization1()
    {
        driver.Url.Should().Be("https://staff-testing.testkontur.ru/news");
    }
    
    [Test] //Тест №2. Переход на страницу с сообществами.
    public void Communities()
    {
        SidebarMenu();
        
        //Поиск элемента "Сообщества" и клик по нему.
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
        community.Click();
        //Проверка наличия заголовка "Сообщества"
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));   
        Assert.That(communityTitle.Text == "Сообщества", "Заголовок должен быть Сообщества");
    }
    
    [Test] //Тест №3. Поиск сотрудника в поисковой строке.
    public void Search()
    {
        //Поиск элемента поисковой строки и клик
        var search = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        search.Click();
        
        string lastName = "Мустафин";
        //После клика появляется рамка, вписываем фамилию
        var input = driver.FindElement(By.CssSelector("[placeholder='Поиск сотрудника, подразделения, сообщества, мероприятия']"));
        input.SendKeys(lastName);
        
        //Явное ожидание, ждем пока прогрузится поисковая строка
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='ComboBoxMenu__item']")));
        
        //Поиск фамилии из выборки
        var namesearch = driver.FindElement(By.CssSelector("[data-tid='ScrollContainer__inner']")).Text;
        namesearch.Should().Contain(lastName);
    }

    [Test] //Тест №4. Создание и удаление сообщества.
    public void CreateCommunity()
    {
        //Переход на страницу "Сообщества" по url.
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");

        //Явное ожидание, чтобы можно было нажать на кнопку.
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='PageHeader']")));
        
        //Ищем кнопку "Создать" и кликаем по ней. Ищем поле с названием, вписываем текст.
        var newCommunity = driver.FindElement(By.XPath("//*[contains(text(),'СОЗДАТЬ')]"));
        newCommunity.Click();
        var createName = driver.FindElement(By.CssSelector("[placeholder='Название сообщества']"));
        createName.SendKeys("Сообщество для автотеста");
        
        //Ищем кнопку "Создать" и кликаем по ней.
        var button = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        button.Click();
        
        //Если сообщество создано, появится текст "Управление сообществом". Ищем его для проверки.
        string namesearch = driver.FindElement(By.XPath("//*[contains(text(),'Управление сообществом')]")).Text;
        namesearch.Should().Contain("Управление сообществом");
        
        //Ищем кнопку удалить и нажимаем на неё.
        var delbatton = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        delbatton.Click();

        //В дополнительном окне подтверждения находим кнопку удаления и нажимаю на нее.
        var delclick = driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] button"));
        delclick.Click();
    }

    [Test] //Тест №5. Изменение дополнительного E-mail
    public void EditAdditionalEmail()
    {
        //Переход по url в настройки профиля и поиск поля допоплнительной почты
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        var additionalEmail = driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail'] input"));
        
        //Стираем всё, что указано в поле и вводим новую почту
        additionalEmail.SendKeys(Keys.Control + "a");
        additionalEmail.SendKeys(Keys.Backspace);
        additionalEmail.SendKeys("selenium@test.ru");
        
        //Ищем кнопку "Сохранить" и нажимаем на неё.
        var saveButton = driver.FindElement(By.CssSelector("[data-tid='PageHeader'] button"));
        saveButton.Click();
        
        //Проверяем, что почта изменилась.
        var contactCard = driver.FindElement(By.CssSelector("[data-tid='ContactCard']"));
        contactCard.Text.Should().Contain("selenium@test.ru");
        }

    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        var login = driver.FindElement(By.Id("Username")); 
        login.SendKeys(" ");
        var password = driver.FindElement(By.Name("Password")); 
        password.SendKeys(" ");
        var enter = driver.FindElement(By.Name("button")); 
        enter.Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }
    public void SidebarMenu()
    {
        //Ищем кнопку боковой панели и кликаем на неё.
        var sidebarMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebarMenu.Click();
    }
    
        [TearDown] 
    public void TearDown() 
    { 
        driver.Close();
        driver.Quit(); 
    } 
}