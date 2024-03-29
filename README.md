  Microsoft.AspNetCore.ResponseCompression.

  Для компрессии ответа в рамках ASP.NET Core мы можем использовать либо внутренние средства конкретного веб-сервера: модуль динамического сжатия (Dynamic Compression module) в IIS, модуль mod_deflate в Apache или инструменты компрессии в Nginx. Однако эти инструменты не всегда бывают доступны, особенно когда приложение напрямую хостится в рамках таких веб-серверов как HttpSys или Kestrel. И в этом случае мы можем воспользоваться специальным компонентом middleware - Microsoft.AspNetCore.ResponseCompression.
  При использовании компрессии надо учитывать обратную сторону компрессии - уменьшение производительности: затраты на компрессию могут превосходить выгоду от уменьшения объема данных. Поэтому, для небольших объемов (меньше 1 кб) не очень рационально применение компрессии.
  
  Провайдеры компрессии.
  
  По умолчанию используется сжатие gzip (класс GzipCompressionProvider). При необходимости можно создавать и применять свои провайдеры сжатия.Для создания провайдера необходимо реализовать интерфейс ICompressionProvider, в котором есть два свойства SupportsFlush и EncodingName и метод CreateStream. 
  Свойство EncodingName указывает на формат сжатия, который поддерживает клиент. Данный формат содержится в заголовке Accept-Encoding в запросе к серверу, наподобие: Accept-Encoding: gzip, deflate, sdch, br.
  Свойство SupportsFlush указывает, поддерживается ли сброс записи в поток.
  Метод CreateStream() возвращает сам поток ответа после сжатия или фактически обертка над изначальным потоком ответа, который передается в качестве параметра в метод. Для сжатия применяется встроенный класс DeflateStream().
  
  
  По умолчанию для протокола HTTPS отключено сжатие, но его можно подключить, использовав EnableForHttps
