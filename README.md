
# HRM 任務流程說明

本系統為多流程、多任務的 HRM 任務處理架構，包含可並行與順序執行的任務類型，流程如下：

---

## 🧭 HRM 多流程任務處理流程圖（含並行/順序標註）

```
📦 Process 1: validate-employee-change （✅ 可並行）
    ├─ task:check-employee-info
    └─ task:check-change-type
         ↓（所有任務完成後進入下一流程）

💵 Process 2: calculate-salary-adjustment （⏳ 順序執行）
    ├─ ① task:fetch-salary-structure
    └─ ② task:calculate-adjusted-salary
         ↓（第二個任務完成後進入下一流程）

📝 Process 3: initiate-approval-flow （⏳ 順序執行）
    ├─ ① task:create-approval-request
    └─ ② task:notify-approver
         ↓（流程完成）

✅ HRM 任務流程結束
```

---

## Redis 任務 Queue 命名

| 任務名稱                         | Redis Queue 名稱                     |
|----------------------------------|--------------------------------------|
| task:check-employee-info         | queue:pending:check-employee-info    |
| task:check-change-type           | queue:pending:check-change-type      |
| task:fetch-salary-structure      | queue:pending:fetch-salary-structure |
| task:calculate-adjusted-salary   | queue:pending:calculate-adjusted-salary |
| task:create-approval-request     | queue:pending:create-approval-request |
| task:notify-approver             | queue:pending:notify-approver        |

---

## 任務執行策略

- ✅ 並行任務流程：所有任務同時加入 queue，由 worker 並行處理。
- ⏳ 順序任務流程：任務完成後，才會啟動下一個任務。

