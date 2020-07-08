<template>
  <div class="app-container">
 <el-table
 v-loading="listLoading"
      :data="list"
      element-loading-text="Loading"
      border
      fit
      stripe
      enable-row-hover 
      enable-row-transition 
      medium
      highlight-current-row
      style="width: 100%">
      <el-table-column 
      fixed 
      prop="code" 
      align="center" 
      label="编号" 
      width="150" >
        <template slot-scope="scope">
          {{ scope.row.taskCode }}
        </template>
      </el-table-column>
    
      <el-table-column label="脚本" width="150">
        <template slot-scope="scope">
          {{ scope.row.scriptCode }}
        </template>
      </el-table-column>
       <el-table-column label="账套"  align="center">
        <template slot-scope="scope">
          {{ scope.row.orgCode }}
        </template>
      </el-table-column>
          <el-table-column
          align="center"
      fixed="right"
      label="操作"
      width="130">
      <template slot-scope="scope">
        <el-button @click="handleClick(scope.row)" type="text" size="small">查看</el-button>
        <el-button type="text" size="small">编辑</el-button>
        <el-button type="text" size="small">删除</el-button>
      </template>
    </el-table-column>
    </el-table>
  </div>
</template>


<script>
import { getTasksDetailList } from '@/api/table'

export default {
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'gray',
        deleted: 'danger'
      }
      return statusMap[status]
    }
  },
  data() {
    return {
      list: null,
      listLoading: true
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      this.listLoading = true
      getTasksDetailList().then(response => {
        this.list = response.data.items
        this.listLoading = false
      })
    }
  }
}
</script>
