/*
 Navicat Premium Data Transfer

 Source Server         : localhost_3306
 Source Server Type    : MySQL
 Source Server Version : 100129
 Source Host           : localhost:3306
 Source Schema         : mysql

 Target Server Type    : MySQL
 Target Server Version : 100129
 File Encoding         : 65001

 Date: 23/05/2024 13:44:43
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account`  (
  `id` text CHARACTER SET latin1 COLLATE latin1_bin NOT NULL,
  `pw` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
  PRIMARY KEY (`id`(20)) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = COMPACT;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('1', '1');
INSERT INTO `account` VALUES ('111', '111');
INSERT INTO `account` VALUES ('112', '112');
INSERT INTO `account` VALUES ('123', '123');
INSERT INTO `account` VALUES ('123456', '123456');
INSERT INTO `account` VALUES ('221', '221');
INSERT INTO `account` VALUES ('222', '222');
INSERT INTO `account` VALUES ('333', '333');
INSERT INTO `account` VALUES ('444', '444');
INSERT INTO `account` VALUES ('555', '555');
INSERT INTO `account` VALUES ('666', '666');
INSERT INTO `account` VALUES ('999', '999');

SET FOREIGN_KEY_CHECKS = 1;
