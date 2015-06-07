namespace CustomWebServer.Logic.Types
{
    /// <summary>
    /// Информация о гостевой книге, которую получает пользователь
    /// </summary>
    public class MessageResult
    {
        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string User { get; set; }
    }
}
