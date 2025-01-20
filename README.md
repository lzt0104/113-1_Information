# 113-1 Information

## 專案概述

這門課是資訊實務實習（一），這學期我利用ASP.NET MVC Core 8進行了購物網站的實作，包含前端與後端。

## 系統需求

- **Visual Studio**：版本 17 或更高。
- **.NET**：最低版本為 .NET 8.0。
- **其他相依性**：
  - Bootstrap
  - jQuery
  - jQuery Validation

## 專案結構

```
C:.
│  114skills.csproj                  # 專案檔案
│  appsettings.Development.json      # 開發環境設定檔
│  appsettings.json                  # 全域設定檔
│  Program.cs                        # 入口程式
│
├─Areas                              # 區域化功能模組
│  ├─Admin                           # 後台管理
│  │  ├─Controllers                  # 控制器
│  │  └─Views                        # 視圖
│  ├─Customer                        # 前台功能
│  │  ├─Controllers                  # 控制器
│  │  └─Views                        # 視圖
│  └─Identity                        # 身分驗證模組
│      └─Pages                       # 身分驗證相關頁面
│
├─bin                                # 編譯輸出
├─Controllers                        # 全域控制器
├─obj                                # 編譯暫存檔
├─Properties                         # 專案屬性
├─Views                              # 全域視圖
│  └─Shared                          # 共用視圖
└─wwwroot                            # 靜態資源
    ├─css                            # CSS 資源
    ├─images                         # 圖片資源
    ├─js                             # JavaScript 資源
    └─lib                            # 外部函式庫
```

## 功能模組

### Admin 區域

- **Category Management**: 新增、編輯、刪除與瀏覽分類。
- **Product Management**: 上傳、編輯與管理產品資訊。
- **Order Management**: 查看與管理客戶訂單。

### Customer 區域

- **Shopping Cart**: 查看與管理購物車內容。
- **Order Confirmation**: 確認與提交訂單。
- **Home Page**: 瀏覽商品詳情與首頁資訊。

### Identity 區域

- 提供登入、註冊、密碼重設與其他身分驗證功能。

## 技術堆疊

- **前端**：Bootstrap, jQuery
- **後端**：ASP.NET Core, Entity Framework Core
- **資料庫**：SQL Server
