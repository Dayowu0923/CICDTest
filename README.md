<template>
  <div style="width: 100%">
    <table1
      @update:page="(page) => (tablePage = page)"
      @update:page-size="(pageSize) => (tablePabeSize = pageSize)"
      :columns="columnsMy"
      :data="data"
      :isShowAll="false"
    ></table1>
  </div>
</template>

<script setup lang="ts">
import { onMounted, h, ref, watch } from "vue";
import table1 from "./components/table1.vue";
import { DataTableBaseColumn } from "naive-ui";
import axios from "axios";

// table頁數
const tablePage = ref(1);
// table一頁顯示數
const tablePabeSize = ref(10);

watch(
  () => tablePage.value,
  async (newValue) => {
    alert(newValue);
  }
);

const columnsMy: DataTableBaseColumn<any>[] = [
  { title: "111", key: "length", width: "30%" },
  { title: "1111", key: "batchNumber", width: "10%" },
  { title: "2222", key: "classPeriod", width: "20%" },
  { title: "3333", key: "signUpResult", width: "10%" },
  { title: "4444", key: "examResult", width: "10%" },
  {
    title: "55555",
    key: "title",
    width: 240,
    render(row) {
      return h(
        "div",
        {
          class: "d-flex gap-2", // 使用 Bootstrap 的 d-flex 和 gap-2 來控制按鈕間距
        },
        [
          // 編輯按鈕
          h(
            "button",
            {
              class: "btn btn-warning", // 編輯按鈕的樣式
              onClick: () => {
                alert(`編輯: ${row.no}`);
              },
            },
            "編輯"
          ), // 按鈕文字

          // 刪除按鈕
          h(
            "button",
            {
              class: "btn btn-danger", // 刪除按鈕的樣式
              onClick: () => {
                alert(`刪除: ${row.no}`);
              },
            },
            "刪除"
          ), // 按鈕文字
        ]
      );
    },
  },
];

interface Song {
  no: number;
  title: string;
  length: string;
}

const data: Song[] = [
  { no: 3, title: "Wonderwall", length: "4:18" },
  { no: 4, title: "Don't Look Back in Anger", length: "4:48" },
  { no: 12, title: "Champagne Supernova", length: "7:27" },
  { no: 3, title: "Wonderwall", length: "4:18" },
  { no: 4, title: "Don't Look Back in Anger", length: "4:48" },
  { no: 12, title: "Champagne Supernova", length: "7:27" },
  { no: 3, title: "Wonderwall", length: "4:18" },
  { no: 4, title: "Don't Look Back in Anger", length: "4:48" },
  { no: 12, title: "Champagne Supernova", length: "7:27" },
  { no: 3, title: "Wonderwall", length: "4:18" },
  { no: 4, title: "Don't Look Back in Anger", length: "4:48" },
  { no: 12, title: "Champagne Supernova", length: "7:27" },
  { no: 3, title: "Wonderwall", length: "4:18" },
  { no: 4, title: "Don't Look Back in Anger", length: "4:48" },
  { no: 12, title: "Champagne Supernova", length: "7:27" },
  { no: 3, title: "Wonderwall", length: "4:18" },
  { no: 4, title: "Don't Look Back in Anger", length: "4:48" },
  { no: 12, title: "Champagne Supernova", length: "7:27" },
];

/* onMounted(async () => {
  let url = "/api/index4"; // 使用相對路徑
  try {
    let data = await axios.put(url);
    alert("成功獲取資料");
  } catch (e) {
    console.error(e);
  }
}); */
</script>

<style scoped></style>
