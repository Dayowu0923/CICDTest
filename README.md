<template>
  <div>
    <n-data-table
      :columns="columns"
      :data="data"
      :row-class-name="rowClassName"
      :row-key="rowKey"
      :row-props="rowProps"
    >
      <template #empty>
        <div class="text-center w-100">
          <div style="font-size: 3rem; color: #ccc">
            <i class="fa-solid fa-file-circle-xmark"></i>
          </div>
          <div style="font-size: 1.2rem; color: #ccc" class="text-secondary">
            尚無資料
          </div>
        </div>
      </template>
    </n-data-table>
    <n-pagination
      v-if="!isShowAll"
      v-model:page="paginationReactive.page"
      v-model:page-size="paginationReactive.pageSize"
      :page-count="pageCount"
      show-size-picker
      :page-sizes="paginationReactive.pageSizes"
      :page-slot="5"
      style="margin-right: 10px"
      @change="handleChange"
      @update:page-size="handleUpdatePageSize"
    />
  </div>
</template>
<script setup lang="ts">
import { computed, reactive } from "vue";
import {
  NDataTable,
  DataTableColumns,
  PaginationProps,
  NPagination,
} from "naive-ui";

interface IIndaxTable {
  columns: DataTableColumns<any> | DataTableColumns<any>;
  data: Object[];
  rowClassName?: (row: any) => string;
  /** 每列的key，多用於選擇框 */
  rowKey?: (row: any) => string;
  /** 全部顯示，不顯示分頁 */
  isShowAll?: boolean;
  /** 設定每個row調用方法*/
  rowProps?: (rowData: any, rowIndex: number) => object;
}

const props = defineProps<IIndaxTable>();

const emit = defineEmits([
  "update:checked-row-keys",
  "update:page",
  "update:page-size",
]);

const paginationReactive = reactive<PaginationProps>({
  page: 1,
  pageSize: 10,
  showSizePicker: true,
  pageSizes: [
    { label: "10 筆", value: 10 },
    { label: "25 筆", value: 25 },
    { label: "50 筆", value: 50 },
    { label: "100 筆", value: 100 },
    { label: "全部", value: 10000000 },
  ],
  /*   onChange: (page: number) => {
    paginationReactive.page = page;
    alert(page);
    emit("update:page", page);
  },
  onUpdatePageSize: (pageSize: number) => {
    paginationReactive.pageSize = pageSize;
    paginationReactive.page = 1;
    alert(pageSize);
    emit("update:page-size", pageSize);
    emit("update:page", paginationReactive.page);
  }, */
  /* prefix: () => {
    return "每頁顯示";
  }, */
  displayOrder: ["size-picker", "pages"],
  pageSlot: 3,
});

const handleChange = (page: number) => {
  paginationReactive.page = page;
  emit("update:page", page);
};

const pageCount = computed(() => {
  const pageSize = paginationReactive.pageSize ?? 10; // 預設值為 10
  return Math.ceil(props.data.length / pageSize);
});

const handleUpdatePageSize = (pageSize: number) => {
  paginationReactive.pageSize = pageSize;
  paginationReactive.page = 1;
  emit("update:page-size", pageSize);
  emit("update:page", 1);
};
</script>
