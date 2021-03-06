﻿/*
Navicat MySQL Data Transfer

Source Server         : 01开发MySQL
Source Server Version : 50615
Source Host           : 10.1.9.33:3306
Source Database       : taskmanager

Target Server Type    : MYSQL
Target Server Version : 50615
File Encoding         : 65001

Date: 2017-08-01 14:14:45
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for Task
-- ----------------------------
DROP TABLE IF EXISTS `Task`;
CREATE TABLE `task` (
  `TaskID` varchar(50) NOT NULL COMMENT '任务ID',
  `TaskName` varchar(100) NOT NULL COMMENT '任务名称',
  `CronExpressionString` varchar(255) NOT NULL COMMENT '运行频率设置',
  `CronRemark` varchar(255) NOT NULL COMMENT '任务运频率中文说明',
  `AssemblyFullName` varchar(255) NOT NULL COMMENT '任务所在DLL对应的程序集名称',
  `ClassFullName` varchar(255) NOT NULL COMMENT '任务所在类',
  `LastRunTime` datetime DEFAULT NULL COMMENT '上一次运行时间',
  `NextRunTime` datetime DEFAULT NULL COMMENT '下一次运行时间',
  `Status` tinyint(4) NOT NULL COMMENT '状态',
  `IsEnabled` tinyint(4) NOT NULL COMMENT '是否启用',
  `Remark` varchar(1000) NOT NULL COMMENT '备注',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `ModifyTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`TaskID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of task
-- ----------------------------
INSERT INTO `Task` VALUES ('Test2017', '测试任务,输出当前时间', '0/10 * * * * ?', '每10秒运行一次', 'DM.Task', 'DM.Task.TaskSet.TestJob', null, null, '0', '1', ' ', '2017-07-25 00:00:00', null);
