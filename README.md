<template>
  <div class="grid-container">
    <div class="button-cell">
      <n-button @click="handleAddBtnClick" color="#b3f9be" class="button">發佈資訊</n-button>
      <n-button @click="handleAddBtnClick2" color="#b3f9be" class="button">發佈資訊1</n-button>
    </div>
    <div class="empty-cell"></div>
    <div class="search-cell">
      <div class="search-div">
        <n-input class="search-input" placeholder="搜尋公告與下載資料">
          <template #prefix>
            <n-icon :component="Search" />
          </template>
        </n-input>
      </div>
    </div>
  </div>

  <div style="color: white; font-weight: bold; margin: 24px 0px; font-size: 18px">
    <n-icon :component="Download" />
    公告區
  </div>

  <!--  公告區Table -->
  <announceTable
    @update:page="(page: number) => (tablePage = page)"
    @update:page-size="(pageSize: any) => (tablePabeSize = pageSize)"
    :columns="columnsMy"
    :data="data"
    :isShowAll="false"
  ></announceTable>

  <div style="color: white; font-weight: bold; margin: 24px 0px; font-size: 18px">
    <n-icon :component="Download" />
    下載區
  </div>

  <!-- 下載區Table -->
  <announceTable
    @update:page="(page: number) => (tablePage = page)"
    @update:page-size="(pageSize: number) => (tablePabeSize = pageSize)"
    :columns="columnsMy"
    :data="data"
    :isShowAll="false"
  ></announceTable>

  <n-modal v-model:show="modelShow" class="md">
    <div class="go-system-notify">
      <n-space class="nspace-style" justify="space-between">
        <n-h3 class="modal_title">發布公告 / 下載區資訊</n-h3>
      </n-space>
      <div class="model-content md" ref="contentLeftRef">
        <div class="container">
          <div class="form-group">
            <label class="form-label">(1) 標題設定</label>
            <n-input id="theme-input" placeholder="請輸入標題" class="form-input"></n-input>
          </div>
          <div class="form-group">
            <label class="form-label">(2) 內容設定</label>
            <n-input id="content-input" placeholder="請輸入內容" class="form-input"></n-input>
          </div>
          <input type="file" @change="onChangeFile" />
          <n-upload :custom-request="customRequest" ref="uploadRef" multiple @change="handleChange" :max="3">
            <n-button color="#b3f9be" class="button"
              >上傳檔案
              <template #icon>
                <n-icon>
                  <Upload />
                </n-icon>
              </template>
            </n-button>
            <br /><br />
            <span style="margin-top: 4px">{{ '1)限制：.ppt、.pptx、.pdf，<28MB' }}</span>
            <br />
            <span style="margin-top: 4px; color: red">{{ '2)重新上傳檔案會將先前資料一併刪除' }}</span>
          </n-upload>
        </div>
      </div>
      <div class="model-footer">
        <n-button ghost color="#5A5858" class="text-white modalbtn" @click="modelShow = false"> 取消發佈 </n-button>
        <n-button type="primary" color="#b3f9be" class="modalbtn" @click="modelShow = false"> 確定發佈 </n-button>
      </div>
      <a id="hrefBellUrl" href="" target="_blank"></a>
    </div>
  </n-modal>
</template>

<script lang="ts" setup>
import { ref, watch, h, onMounted } from 'vue'
import {
  NIcon,
  NModal,
  NButton,
  NSpace,
  NH3,
  UploadCustomRequestOptions,
  NUpload,
  UploadFileInfo,
  UploadInst
} from 'naive-ui'
import { Download, Document, Delete, Edit, Search, Upload } from '@vicons/carbon'
import { CloseCircle } from '@vicons/ionicons5'
import searchBar from '@/components/announcement/searchBar.vue'
import announceTable from '@/components/announcement/announceTable.vue'
import { querylist, post } from '@/api/path/index'
// table頁數
const tablePage = ref(1)
// table一頁顯示數
const tablePabeSize = ref(10)

const modelShow = ref(false)
const aaaa = ref()
/* watch(
  () => tablePage.value,
  async newValue : => {
    alert(newValue)
  }
)
 */
const columnsMy: any[] = [
  { title: '發佈日期', key: 'publishDate', width: '20%' },
  { title: '標題', key: 'title', width: '20%' },
  { title: '上傳者', key: 'classPeriod', width: '20%' },
  { title: '內容', key: 'content', width: '20%' },
  {
    title: '功能',
    key: 'title',
    width: 240,
    render(row: { announcementBoardId: any }) {
      return h(
        'div',
        {
          class: 'button-group',
          style: {
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            gap: '2px',
            flexWrap: 'nowrap'
          }
        },
        [
          h(
            'div',
            {
              class: 'custom-btn danger',
              style: {
                display: 'flex',
                alignItems: 'center',
                padding: '5px 10px',
                color: '#fff',
                borderRadius: '4px',
                cursor: 'pointer'
              },
              onClick: () => {}
            },
            [h(NIcon, { component: Delete, style: { marginRight: '12px' } }), '刪除']
          ),
          h(
            'div',
            {
              class: 'custom-btn warning',
              style: {
                display: 'flex',
                alignItems: 'center',
                padding: '5px 10px',
                color: '#fff',
                borderRadius: '4px',
                cursor: 'pointer'
              },
              onClick: () => {
                alert(`編輯: ${row.announcementBoardId}`)
              }
            },
            [h(NIcon, { component: Edit, style: { marginRight: '12px' } }), '編輯']
          ),
          h(
            'div',
            {
              class: 'custom-btn warning',
              style: {
                display: 'flex',
                alignItems: 'center',
                padding: '5px 10px',
                color: 'rgb(145,224,157)',
                borderRadius: '4px',
                cursor: 'pointer'
              },
              onClick: () => {
                alert(`檔案: ${row.announcementBoardId}`)
              }
            },
            [h(NIcon, { component: Document, style: { marginRight: '12px' } }), '檔案']
          )
        ]
      )
    }
  }
]

interface iData {
  announcementBoardId: string
  publishDate: string
  title: string
  content: string
}

const data: iData[] = [
  { announcementBoardId: '3', title: 'Wonderwall', content: '1.資本預算注意事項', publishDate: '2024-01-01' },
  {
    announcementBoardId: '4',
    title: "Don't Look Back in Anger",
    content: '1.資本預算注意事項',
    publishDate: '2024-01-01'
  },
  { announcementBoardId: '12', title: 'Champagne Supernova', content: '1.資本預算注意事項', publishDate: '2024-01-01' },
  { announcementBoardId: '3', title: 'Wonderwall', content: '1.資本預算注意事項', publishDate: '2024-01-01' },
  {
    announcementBoardId: '4',
    title: "Don't Look Back in Anger",
    content: '1.資本預算注意事項',
    publishDate: '2024-01-01'
  }
]

const uploadRef = ref<UploadInst | null>(null)

const customRequest = ({
  file,
  data,
  headers,
  withCredentials,
  action,
  onFinish,
  onError
}: UploadCustomRequestOptions) => {
  const formData = new FormData()
  if (data) {
    Object.keys(data).forEach(key => {
      formData.append(key, data[key as keyof UploadCustomRequestOptions['data']])
    })
    console.log(`目前帶入的form_no是：${(data as Record<string, string>)['form_no']}`)
  }
  formData.append('file', file.file as File, file.name)

  /*  let url = `${import.meta.env.PROD ? import.meta.env.VITE_PRO_PATH : ''}${axiosPre}/File/UploadFile`
  console.log(`naive action: ${action}, fixed url: ${url}`)

  axios({
    method: 'post',
    // url: action as string,
    url: url,
    data: formData,
    headers: {
      ...(headers as Record<string, string>),
      'Content-Type': 'multipart/form-data'
    },
    withCredentials: withCredentials,
    onUploadProgress: () => {}
  })
    .then((response: any) => {
      if (response.status === 200) {
        console.log('Upload Response:', response.data)

        if (response?.data?.data.message) {
          message.warning(response.data.data.message, {
            closable: true,
            duration: 8000
          })
        } else {
          uploadInfo.value = response.data.data
          message.success('上傳成功')
        }
      }

      isUploading.value = false
      onFinish()
    })
    .catch((error: any) => {
      message.error('上傳檔案發生錯誤')

      if (error.response) {
        console.error('Response Error Data:', error.response.data)
        console.error('Response Error Status:', error.response.status)
        console.error('Response Headers:', error.response.headers)
      } else {
        console.error('Error Message:', error.message)
      }

      isUploading.value = false
      onError()
    }) */
}

onMounted(async () => {
  try {
    const res = await querylist()
    console.log(res)
  } catch (e) {}
})

const fileListRef = ref<UploadFileInfo[]>()
const onChangeFile = (event: any) => {
  console.log(event.target.files[0])
}

const handleChange = (data: { fileList: UploadFileInfo[] }) => {
  fileListRef.value = data.fileList
  data.fileList.forEach(x => {
    if (x.file?.size != undefined && x.file?.size > 10 * 1024 * 1024) {
      alert('超過10MB')
    }
  })
}

//
const handleAddBtnClick = () => {
  console.log(fileListRef.value)
  modelShow.value = true
}

const handleAddBtnClick2 = async () => {
  const formData = new FormData()
  fileListRef.value?.forEach(item => {
    if (item.file && item.file instanceof File) {
      formData.append('files[]', item.file, item.name)
    }
  })
  formData.append('Title', '2222')
  formData.append('Content', '1111')
  formData.append('PublishDate', '2024-01-01')
  const res = await post(formData)
}
</script>

<style scoped>
.md .go-system-notify {
  padding: 36px;
}

.nspace-style {
  background-color: rgb(42, 42, 43);
  display: block !important;
}
.md {
  max-width: 1024px;
  width: 100%;
  position: relative;
}
.modal_title {
  padding: 24px 12px;
  margin: 0;
  text-align: center;
}
.model-content {
  min-height: 768px;
  background-color: rgb(30, 30, 31);
}

.mdPr {
  position: relative;
}

.model-footer {
  position: absolute;
  bottom: 40px;
  right: 80px;
  display: flex;
  gap: 40px;
  z-index: 10;
}

.model-footer n-button {
  margin: 0;
}

.grid-container {
  display: grid;
  grid-template-columns: repeat(12, 1fr);
  width: 100%;
}
.button-cell {
  grid-column: span 1;
  padding: 8px;
}
.empty-cell {
  grid-column: span 2;
}

.search-cell {
  grid-column: span 8;
  display: flex;
  justify-content: flex-end;
  padding: 8px;
}

.container {
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 600px;
  width: 100%;
}

.theme,
.content {
  display: flex;
  align-items: center;
}

label {
  width: 50px;
  text-align: right;
  margin-right: 10px;
}

.textbot {
  flex: 1;
  padding: 5px 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

.form-label {
  flex-shrink: 0;
  text-align: left;
  color: white;
  font-size: 14px;
  padding-left: 20px;
}

.form-group {
  width: 100%;
  margin-bottom: 16px;
}

.form-input {
  width: 100%;
}

.text-white {
  color: white !important;
}

.cancel-button {
  background-color: #5a5858 !important;
  color: white !important;
  border-color: #5a5858 !important;
}

.cancel-button:hover {
  background-color: #474747;
  color: white;
}
.search-div {
  width: 100%;
}

.search-input {
  background-color: #18181c;
  color: #fff;
  width: 100%;
}

.modalbtn {
  padding: 12px 24px;
}
</style>



------------------------------------


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
          <div style="font-size: 1.2rem; color: #ccc" class="text-secondary">尚無資料</div>
        </div>
      </template>
    </n-data-table>
    <n-pagination
      v-if="!isShowAll"
      v-model:page="paginationReactive.page"
      v-model:page-size="paginationReactive.pageSize"
      :page-count="pageCount"
      :page-sizes="paginationReactive.pageSizes"
      :page-slot="5"
      style="margin-top: 16px; float: right"
      @update="handleChange"
      @update:page-size="handleUpdatePageSize"
    />
  </div>
</template>
<script setup lang="ts">
import { computed, reactive } from 'vue'
import { NDataTable, DataTableColumns, PaginationProps, NPagination } from 'naive-ui'

interface IIndaxTable {
  columns: DataTableColumns<any> | DataTableColumns<any>
  data: Object[]
  rowClassName?: (row: any) => string
  /** 每列的key，多用於選擇框 */
  rowKey?: (row: any) => string
  /** 全部顯示，不顯示分頁 */
  isShowAll?: boolean
  /** 設定每個row調用方法*/
  rowProps?: (rowData: any, rowIndex: number) => object
}

const props = defineProps<IIndaxTable>()

const emit = defineEmits(['update:checked-row-keys', 'update:page', 'update:page-size'])

const paginationReactive = reactive<PaginationProps>({
  page: 1,
  pageSize: 10,
  showSizePicker: true,
  pageSizes: [
    { label: '10 筆', value: 10 },
    { label: '25 筆', value: 25 },
    { label: '50 筆', value: 50 },
    { label: '100 筆', value: 100 },
    { label: '全部', value: 10000000 }
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
  displayOrder: ['size-picker', 'pages'],
  pageSlot: 3
})

const handleChange = (page: number) => {
  paginationReactive.page = page
  emit('update:page', page)
}

const pageCount = computed(() => {
  const pageSize = paginationReactive.pageSize ?? 10 // 預設值為 10
  return Math.ceil(props.data.length / pageSize)
})

const handleUpdatePageSize = (pageSize: number) => {
  paginationReactive.pageSize = pageSize
  paginationReactive.page = 1
  emit('update:page-size', pageSize)
  emit('update:page', 1)
}
</script>
