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

 Date: 23/05/2024 13:45:12
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for roomlog
-- ----------------------------
DROP TABLE IF EXISTS `roomlog`;
CREATE TABLE `roomlog`  (
  `RoomID` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `RegisterTime` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
  `EnterMsg` text CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL,
  PRIMARY KEY (`RoomID`(20)) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Records of roomlog
-- ----------------------------
INSERT INTO `roomlog` VALUES ('0', '2024/5/10 0:04:45', '');
INSERT INTO `roomlog` VALUES ('1', '2024/5/10 0:04:50', '');
INSERT INTO `roomlog` VALUES ('10', '2024/5/10 4:18:00', NULL);
INSERT INTO `roomlog` VALUES ('11', '2024/5/10 5:15:17', NULL);
INSERT INTO `roomlog` VALUES ('12', '2024/5/10 5:56:27', NULL);
INSERT INTO `roomlog` VALUES ('13', '2024/5/10 17:29:03', '(123456)(13)');
INSERT INTO `roomlog` VALUES ('14', '2024/5/10 17:31:25', '(123)');
INSERT INTO `roomlog` VALUES ('15', '2024/5/19 2:54:00', '(123)');
INSERT INTO `roomlog` VALUES ('16', '2024/5/19 6:04:08', '(999)(16)');
INSERT INTO `roomlog` VALUES ('2', '2024/5/10 0:08:19', '(123)');
INSERT INTO `roomlog` VALUES ('3', '2024/5/10 0:08:22', '(123)');
INSERT INTO `roomlog` VALUES ('4', '2024/5/10 3:07:37', '(123)');
INSERT INTO `roomlog` VALUES ('5', '2024/5/10 3:08:13', NULL);
INSERT INTO `roomlog` VALUES ('6', '2024/5/10 3:08:42', '(222)');
INSERT INTO `roomlog` VALUES ('7', '2024/5/10 3:45:09', '(123)');
INSERT INTO `roomlog` VALUES ('8', '2024/5/10 4:16:43', NULL);
INSERT INTO `roomlog` VALUES ('9', '2024/5/10 4:17:18', NULL);

SET FOREIGN_KEY_CHECKS = 1;
