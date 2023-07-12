# ExternalServiceHandler
# Perfect Money Robot ---[Telgram Bot (webhook)](https://github.com/AmBplus/ExternalServiceHandler](https://github.com/AmBplus/TelegramBot_PerfectMoney_2)
میکرسرویس پرداخت وریفای 
سرویس فینوتک و زیبال توسط این میکروسرویس ساپورت میشود

موارد کلیدی پروژه
- [x] Asp.net Core
- [x] Minimal Api
- [x] CQRS
- [x] Rest
- [x] Rest Sharp
- [x] Connect To External Api
- [x] Result Pattern
- [x] Zibal
- [x] Finotech

هدف از این برنامه اتصال به سرویس های خارجی مورد نیاز برنامس ، از جمله اتصال به درگاه پرداخت ، سرویس فینوتک برای احراز هویت شماره کارت و شماره همراه 
این برنامه با Minimal Api ها پیاده سازی شده
این برنامه به صورت تعاملی برای ربات پرفکت مانی ساخته شده ، به صورتی که با گرفتن درخواست از سمت ربات که در یک سرور کاملا جدا قرار دارد ، درخواست قبول کرده ، به سامانه مورد انتظار متصل شده و نتیجه را بر میگرداند
در مواردی مانند وریفای پرداخت ، سامانه زیبال به برنامه متصل شده ، سپس ما از طریق برنامه دوباره به ربات تلگرام که سرور دیگریست متصل میشویم ، بررسی میکنیم این پرداخت صحیح است یا خیر ، مثلا در مواردی زمان پرداخت از منقضی شده و ممکن است قیمت محصول ما تغیر کرده باشد ، در اینگونه موارد ، حتی اگر پرداخت موفقیت آمیز بوده باشد ، اگر زمان پرداخت گذشته باشد ، ما پرداخت را تایید نمیکنیم و برگشت میخورد ، بعد از بررسی ، دوباره به سامانه زیبال متصل شده و پرداخت را وریفای میکنیم ، سپس دوباره نتیجه را به برنامه تلگرام ارسال میکنیم 
