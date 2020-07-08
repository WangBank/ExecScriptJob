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
          {{ scope.row.code }}
        </template>
      </el-table-column>
    
      <el-table-column label="账套名称" width="150">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
      <el-table-column label="账套类型" width="110" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.dbType }}</span>
        </template>
      </el-table-column>
      <el-table-column label="数据库类型" width="110" align="center">
        <template slot-scope="scope">
          {{ scope.row.dbType }}
        </template>
      </el-table-column>
      <!-- <el-table-column class-name="status-col" label="Status" width="110" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.status | statusFilter">{{ scope.row.status }}</el-tag>
        </template>
      </el-table-column> -->
      <el-table-column align="center" prop="created_at" label="数据库地址" width="200">
        <template slot-scope="scope">
          <!-- <i class="el-icon-time" /> -->
          <span>{{ scope.row.serverName }}</span>
        </template>
      </el-table-column>

       <el-table-column label="用户名" width="110" align="center">
        <template slot-scope="scope">
          {{ scope.row.userName }}
        </template>
      </el-table-column>
       <el-table-column label="数据库名称" width="130" align="center">
        <template slot-scope="scope">
          {{ scope.row.dataBaseName }}
        </template>
      </el-table-column>
       <el-table-column label="连接字符串" width="800" align="center">
        <template slot-scope="scope">
          {{ scope.row.connectingString }}
        </template>
      </el-table-column>
       <el-table-column label="账套描述" width="110" align="center">
        <template slot-scope="scope">
          {{ scope.row.description }}
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
import { getOrganizationList } from '@/api/table'

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
      getOrganizationList().then(response => {
        this.list = response.data.items
        this.listLoading = false
      })
    }
  }
}
</script>
