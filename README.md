sample-aspnet-api
----

※現在開発中

sample-boot-hibernate 同様に API 機能のみ提供するサンプル実装です。

## 環境構築手順

### Windows での環境構築手順

> 開発時は Visual Studio 2015 を入れてしまう手順 ( Install ASP.NET 5 with Visual Studio ) が簡単でオススメです。

https://docs.asp.net/en/latest/getting-started/installing-on-windows.html

環境構築後に Sample.sln をダブルクリックで Visual Studio 2015 からアプリケーションを実行できます。

### Mac での環境構築手順

> 開発時は Visual Studio Code を入れてしまう手順 ( Install ASP.NET 5 with Visual Studio Code ) が良いですが、Mac 上での実行確認だけならコンソールベース ( Install ASP.NET 5 from the command-line ) でも十分です。

https://docs.asp.net/en/latest/getting-started/installing-on-mac.html

環境構築後に src/Sample 直下へ移動して、 `dnx web` を実行すればアプリケーションが起動します。  
※テスト確認したいなら test/Sample.Test 直下へ移動して、 `dnx test` を実行してください。

## TODO

- 認証系(Identity)の追加
- キャッシングの追加
- 暗号化絡みのサポート
- Repository絡みで起こるであろうリソースリークを想定してライフサイクルでの正確な挙動を調べる
- 非同期系の考慮
