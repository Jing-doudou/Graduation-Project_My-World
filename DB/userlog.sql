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

 Date: 23/05/2024 13:45:23
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for userlog
-- ----------------------------
DROP TABLE IF EXISTS `userlog`;
CREATE TABLE `userlog`  (
  `UserID` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `RegisterTime` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
  `LastLoginTime` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
  PRIMARY KEY (`UserID`(20)) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Records of userlog
-- ----------------------------
INSERT INTO `userlog` VALUES ('1', '2024/5/10 0:03:30', '\"2024/5/10 0:03:31\"');
INSERT INTO `userlog` VALUES ('123', '2024/5/10 0:04:39', '\"2024/5/19 6:04:18\"');
INSERT INTO `userlog` VALUES ('123456', '2024/5/10 17:28:59', '\"2024/5/10 17:29:01\"');
INSERT INTO `userlog` VALUES ('222', '2024/5/10 3:08:24', '\"2024/5/10 5:56:31\"');
INSERT INTO `userlog` VALUES ('999', '2024/5/19 6:03:54', '\"2024/5/19 6:04:01\"');

SET FOREIGN_KEY_CHECKS = 1;
