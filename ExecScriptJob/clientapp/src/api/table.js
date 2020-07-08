import request from '@/utils/request'

export function getOrganizationList(params) {
  return request({
    url: '/organization/info',
    method: 'get',
    params
  })
}

export function getScriptList(params) {
  return request({
    url: '/script/info',
    method: 'get',
    params
  })
}

export function getTasksList(params) {
  return request({
    url: '/tasks/info',
    method: 'get',
    params
  })
}

export function getTasksDetailList(params) {
  return request({
    url: '/tasksDetail/info',
    method: 'get',
    params
  })
}
